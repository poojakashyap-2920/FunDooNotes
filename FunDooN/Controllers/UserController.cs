using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration; // Import IConfiguration namespace
using BusinessLayer.UserInterface;
using RepositoryLayer.Entity;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using RepositoryLayer.Filter;
using RepositoryLayer.UserEntityService;
using Microsoft.Extensions.Caching.Distributed;
using Confluent.Kafka;
using Newtonsoft.Json; // Import Task namespace

namespace FunDooN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserBl _userBl;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache distributedCache;
        private readonly ProducerConfig _config;

        //note
        private readonly ICobllabBl _noteBl;


        public UserController(IUserBl userBl, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ICobllabBl noteBl, IDistributedCache distributedCache,ProducerConfig _config)
        {
            _userBl = userBl;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _noteBl = noteBl;
            this.distributedCache = distributedCache; // Initialize distributedCache field
            this._config = _config;
        }


        //insert
        [HttpPost("UserRegistration")]
        public async Task<IActionResult> InsertUser(string topic,[FromBody] UserEntity empDto)
        {
            try
            {
                // Producer
                string serializedUser = JsonConvert.SerializeObject(empDto);
                using (var producer = new ProducerBuilder<Null, string>(_config).Build())
                {
                    await producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedUser });
                }
                // Inserting user details into the database
                await _userBl.InsertUser(empDto.FirstName, empDto.LastName, empDto.EmailId,empDto.Password);
              //  logger.LogInformation("Registration done successfully");
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
               // logger.LogError($"{ex.Message} exception occurred");
                return BadRequest(ex.Message);
            }

        }


        //login

        /*        [HttpPost("Login")]
                public async Task<IActionResult> Login(string emailid, string password)
                {
                    try
                    {
                        var user = await _userBl.Login(emailid, password);
                        if (user != null)
                        {
                            string token = GenerateJwtToken(emailid);
                            _httpContextAccessor.HttpContext.Session.SetString("Token", token);


                            return Ok(new { Token = token, Email = emailid, Message = "Login successful" });
                        }
                        else
                        {
                            // If user does not exist or credentials are incorrect, return BadRequest
                            return BadRequest("Invalid email or password");
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, "An error occurred while logging in");
                    }
                }*/



        // Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string emailid, string password)
        {
            try
            {
                var user = await _userBl.Login(emailid, password);
                if (user != null)
                {
                    // Generate JWT token
                    string token = GenerateJwtToken(emailid);

                    // Store token in session
                    _httpContextAccessor.HttpContext.Session.SetString("Token", token);

                    return Ok(new { Token = token, Email = emailid, Message = "Login successful" });
                }
                else
                {
                    // If user does not exist or credentials are incorrect, return BadRequest
                    return BadRequest("Invalid email or password");
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while logging in");
            }
        }



        //generate token
        private string GenerateJwtToken(string emailid)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claim = new[]
            {
                new Claim(ClaimTypes.Email, emailid),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claim,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:Minutes"])),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //get all result
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var employees = await _userBl.GetUser();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, ex.Message);
            }
        }


        //get result by id
        [HttpGet("GetAllByEmail")]
        public async Task<IActionResult> GetEmployeeByEmail(string emailid)
        {
            try
            {
                var employee = await _userBl.GetEmployeeByEmail(emailid);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("Update")]
        public async Task<IActionResult> UpdateUserByEmail(string emailid, [FromBody] UserEntity updateDto)
        {
            try
            {
                if (updateDto == null)
                {
                    return BadRequest("Invalid data provided");
                }

                await _userBl.UpdateUserByEmail(emailid, updateDto.FirstName, updateDto.LastName);
                return Ok("User updated successfully");
            }
            catch (Exception e)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while updating the user");
            }
        }

        //delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteEmployeeByEmail(string emailid)
        {
            try
            {
                if (string.IsNullOrEmpty(emailid))
                {
                    return BadRequest("Email ID cannot be null or empty.");
                }

                var userToDelete = await _userBl.GetEmployeeByEmail(emailid);
                if (userToDelete == null)
                {
                    return BadRequest("User with the provided email ID does not exist.");
                }

                await _userBl.DeleteEmployeeByEmail(emailid);
                return Ok("User deleted successfully.");
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, "An error occurred while deleting the user: " + ex.Message);
            }
        }



        //lastname

        [HttpGet("LastName")]
        public async Task<IActionResult> GetLastName()
        {
            try
            {
                var employees = await _userBl.GetLastName();

                return Ok(employees);
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, ex.Message);
            }
        }

        //orderbyname

        [HttpGet("orderbyname")]
        public async Task<IActionResult> OrederByFirstName()
        {
            try
            {
                var employee = await _userBl.OrederByFirstName();
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        //forgot password
        [HttpPut("forgotpass/{Email}")]
        [UserExceptionHandlerFilter]
        public async Task<IActionResult> ChangePasswordRequest(String Email)
        {
            return Ok(await _userBl.ChangePasswordRequest(Email));
        }



        [HttpPut("otp/{otp}/{password}")]
        [UserExceptionHandlerFilter]
        public async Task<IActionResult> ChangePassword(String otp, String password)
        {
            return Ok(await _userBl.ChangePassword(otp, password));
        }
        //***************************
        [HttpPut("{userEmailUpdate}")]
        //[HttpPut("reset-password/{personEmailUpdate}")]
        public async Task<IActionResult> ResetPasswordByEmail(string personEmailUpdate, [FromBody] UserEntity updateDto)
        {
            try
            {
                var user = await _userBl.GetEmployeeByEmail(personEmailUpdate);
                if (user != null)
                {
                    await _userBl.ResetPasswordByEmail(personEmailUpdate, updateDto.Password);
                    return Ok("User password updated successfully");
                }
                else
                {
                    return NotFound("No user found with the provided email");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to reset password: {ex.Message}");
            }
        }

        //session

        // Logout
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            // Remove token from session on logout
            _httpContextAccessor.HttpContext.Session.Remove("Token");
            return Ok("Logged out successfully");
        }

        // Note ****************************************



        [HttpGet("getByIdUsingRedis")]
        public async Task<IActionResult> GetByIdUsingRedis(string email)
        {

            string cacheKey = email;
            var cachedLabel = await distributedCache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedLabel))
            {

                return Ok(System.Text.Json.JsonSerializer.Deserialize<List<UserEntity>>(cachedLabel));

            }
            var labels = await _userBl.GetEmployeeByEmail(email);
            if (labels != null)
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)

                };
                await distributedCache.SetStringAsync(cacheKey, System.Text.Json.JsonSerializer.Serialize(labels), cacheOptions);
                return Ok(labels);
            }
            return NotFound("No id found");


        }

        
        }

    }






