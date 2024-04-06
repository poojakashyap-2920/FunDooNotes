// CollaborationService.cs
using Dapper;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.IUserEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RepositoryLayer.UserEntityService
{
    public class CollaborationService : ICollaboration
    {
        private readonly UserContext _context;

        public CollaborationService(UserContext context)
        {
            _context = context;
        }

        // Add collaboration
        public async Task<bool> AddCollaborator(int collabnoteid, string useremail, int collaborationId)
        {
            var checkQuery = "SELECT COUNT(*) FROM Collaboration WHERE collabnoteid = @CollabNoteId AND useremail = @UserEmail";
            var insertQuery = "INSERT INTO Collaboration(collaborationId, collabnoteid, useremail) VALUES (@CollaborationId, @CollabNoteId, @UserEmail)";

            var parameters = new DynamicParameters();
            parameters.Add("@CollabNoteId", collabnoteid, DbType.Int32);
            parameters.Add("@UserEmail", useremail, DbType.String);
            parameters.Add("@CollaborationId", collaborationId, DbType.Int32);

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    // Check if the collaboration already exists
                    int existingCollaborations = await connection.ExecuteScalarAsync<int>(checkQuery, parameters);
                    Console.WriteLine($"Existing collaborations: {existingCollaborations}");

                    if (existingCollaborations > 0)
                    {
                        // Collaboration already exists
                        Console.WriteLine("Collaboration already exists.");
                        return false;
                    }

                    // Add collaboration
                    await connection.ExecuteAsync(insertQuery, parameters);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding collaborator: " + ex.Message);
                throw; // Rethrow the exception to propagate it upwards
            }
        }

        // Get all collaborators
        public async Task<IEnumerable<Collaboration>> GetAllCollaborators()
        {
            var query = "SELECT * FROM Collaboration";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Collaboration>(query);
            }
        }

        // Remove collaboration
        public async Task<bool> RemoveCollaborator(int collabnoteid, string useremail)
        {
            try
            {
                var deleteQuery = "DELETE FROM Collaboration WHERE collabnoteid = @CollabNoteId AND useremail = @UserEmail";

                var parameters = new DynamicParameters();
                parameters.Add("@CollabNoteId", collabnoteid, DbType.Int32);
                parameters.Add("@UserEmail", useremail, DbType.String);

                using (var connection = _context.CreateConnection())
                {
                    int rowsAffected = await connection.ExecuteAsync(deleteQuery, parameters);
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while removing collaborator: " + ex.Message);
                return false;
            }
        }
    }
}
