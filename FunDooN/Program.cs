

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using BusinessLayer.UserInterface;
using RepositoryLayer.Entity;
using RepositoryLayer.UserEntityService;
using Microsoft.Extensions.Caching.Distributed;
using BusinessLayer.service;
using RepositoryLayer.Context;
using RepositoryLayer.IUserEntity;
using Microsoft.OpenApi.Models;
using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UserContext>();
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IUserBl, IUserServiceBl>();
builder.Services.AddScoped<ICobllabBl, NoteServiceBl>();
builder.Services.AddScoped<INotes, NoteService>();
builder.Services.AddScoped<ICollaboration, CollaborationService>();
builder.Services.AddScoped<ICollabBl, CollabServiceBl>();
builder.Services.AddScoped<ILabelBl, LabelServiceBl>();
builder.Services.AddScoped<ILabel, LabelService>();
builder.Services.AddScoped<NotesEntity>();
builder.Services.AddScoped<UserEntity>();
builder.Services.AddScoped<Collaboration>();

builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    config.AddDebug();
});

builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = builder.Configuration["RedisCacheUrl"]; });

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            },
            Array.Empty<string>()
        }
    });
});



builder.Services.AddControllers();
// Kafka Producer Configuration
var producerConfig = new ProducerConfig();
builder.Configuration.GetSection("producer").Bind(producerConfig);
builder.Services.AddSingleton(producerConfig);


//  kafka  //

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.MapControllers();
app.Run();





