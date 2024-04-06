using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.IUserEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.UserEntityService
{
    public class NoteService : INotes
    {

        private readonly UserContext _context;
        private readonly NotesEntity _note;

        public NoteService(UserContext context, NotesEntity note)
        {
            _context = context;
            _note = note;

        }




        //insert
        public async Task InsertNote(string title, string description, string colour, bool isArchived, bool isDeleted, string emailId)
        {
            string insertQuery = @"
                INSERT INTO NotesEntity (Title, Description, Colour, IsArchived, IsDeleted, EmailId)
                VALUES (@Title, @Description, @Colour, @IsArchived, @IsDeleted, @EmailId);";
            var Parameter = new DynamicParameters();

            Parameter.Add("@Title", title, DbType.String);
            Parameter.Add("@Description", description, DbType.String);
            Parameter.Add("@Colour", colour, DbType.String);

            Parameter.Add("@IsArchived", isArchived, DbType.Boolean);
            Parameter.Add("@IsDeleted", isDeleted, DbType.Boolean);
            Parameter.Add("@EmailId", emailId, DbType.String);



            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(insertQuery, Parameter);

            }
        }

        // get all record
        public async Task<IEnumerable<NotesEntity>> GetNote()
        {
            var query = "SELECT * FROM NotesEntity";
            using (var connection = _context.CreateConnection())
            {
                var note = await connection.QueryAsync<NotesEntity>(query);
                return note.ToList();
            }

        }
        //get note by noteid
        public  async Task<IEnumerable<NotesEntity>> GetNotesById(int NoteId)
        {
            var query = "SELECT * FROM NotesEntity WHERE NoteId = @NoteId";
            using (var connection = _context.CreateConnection())
            {
                var note = await connection.QueryAsync<NotesEntity>(query, new {NoteId = NoteId });
                return note.ToList();
            }
        }

        //delete Note by NoteId
        public async Task DeleteNoteById(int noteid)
        {
            var query = "DELETE FROM NotesEntity WHERE noteid = @NoteId";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { NoteId = noteid });
            }
        }
        //update
        // Update method in NoteService
        public async Task UpdateNoteByNoteId(int noteId, NotesEntity notesEntity)
        {
            var updateQuery = @"
        UPDATE NotesEntity 
        SET Title = @Title, 
            Description = @Description, 
            Colour = @Colour, 
            IsArchived = @IsArchived, 
            IsDeleted = @IsDeleted, 
            EmailId = @EmailId
        WHERE NoteId = @NoteId";

            var parameters = new DynamicParameters();
            parameters.Add("@NoteId", noteId, DbType.Int32);
            parameters.Add("@Title", notesEntity.Title, DbType.String);
            parameters.Add("@Description", notesEntity.Description, DbType.String);
            parameters.Add("@Colour", notesEntity.Colour, DbType.String);
            parameters.Add("@IsArchived", notesEntity.IsArchived, DbType.Boolean);
            parameters.Add("@IsDeleted", notesEntity.IsDeleted, DbType.Boolean);
            parameters.Add("@EmailId", notesEntity.EmailId, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(updateQuery, parameters);
            }
        }

    }
}
    

