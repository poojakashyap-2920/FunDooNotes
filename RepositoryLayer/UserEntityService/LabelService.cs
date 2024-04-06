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
    public class LabelService : ILabel
    {
        private readonly UserContext _context;

        public LabelService(UserContext context)
        {
            _context = context;
        }

        // Insert
        public async Task<bool> AddLabelName(int collabnoteid, string useremail, string labelname)
        {
            var checkQuery = "SELECT COUNT(*) FROM LabelEntity WHERE collabnoteid = @collabnoteid AND useremail = @useremail AND labelname = @labelname";
            var insertQuery = "INSERT INTO LabelEntity(collabnoteid, useremail, labelname) VALUES (@collabnoteid, @useremail, @labelname)";

            var parameters = new DynamicParameters();
            parameters.Add("@collabnoteid", collabnoteid, DbType.Int32);
            parameters.Add("@useremail", useremail, DbType.String);
            parameters.Add("@labelname", labelname, DbType.String);

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    int existingLabels = await connection.ExecuteScalarAsync<int>(checkQuery, parameters);

                    if (existingLabels == 0)
                    {
                        await connection.ExecuteAsync(insertQuery, parameters);
                        return true;
                    }
                    else
                    {
                        // Label with the same collabnoteid, useremail, and labelname already exists
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("An error occurred: " + ex.Message);
                throw; // Rethrow the exception to propagate it upwards
            }
        }

        // Check if a label already exists
        public async Task<bool> CheckQuery(int collabnoteid, string useremail, string labelname)
        {
            var checkQuery = "SELECT COUNT(*) FROM LabelEntity WHERE collabnoteid = @collabnoteid AND useremail = @useremail AND labelname = @labelname";

            var parameters = new DynamicParameters();
            parameters.Add("@collabnoteid", collabnoteid, DbType.Int32);
            parameters.Add("@useremail", useremail, DbType.String);
            parameters.Add("@labelname", labelname, DbType.String);

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    int existingLabels = await connection.ExecuteScalarAsync<int>(checkQuery, parameters);
                    return existingLabels > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("An error occurred: " + ex.Message);
                throw; // Rethrow the exception to propagate it upwards
            }
        }

        // Get all labels
        public async Task<IEnumerable<LabelEntity>> GetAllLabel()
        {
            var query = "SELECT * FROM LabelEntity";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<LabelEntity>(query);
            }
        }

        // Remove label
        public async Task<bool> RemoveLabelName(int collabnoteid, string useremail)
        {
            try
            {
                var deleteQuery = "DELETE FROM LabelEntity WHERE collabnoteid = @collabnoteid AND useremail = @useremail";

                var parameters = new DynamicParameters();
                parameters.Add("@collabnoteid", collabnoteid, DbType.Int32);
                parameters.Add("@useremail", useremail, DbType.String);

                using (var connection = _context.CreateConnection())
                {
                    int rowsAffected = await connection.ExecuteAsync(deleteQuery, parameters);
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        //update
        // Update label
        public async Task<bool> UpdateLabel(int collabnoteid, string useremail, string LabelName)
        {
            try
            {
                var updateQuery = "UPDATE LabelEntity SET labelname = @LabelName WHERE collabnoteid = @collabnoteid AND useremail = @useremail";

                var parameters = new DynamicParameters();
                parameters.Add("@collabnoteid", collabnoteid, DbType.Int32);
                parameters.Add("@useremail", useremail, DbType.String);
                parameters.Add("@LabelName", LabelName, DbType.String); // Corrected parameter name

                using (var connection = _context.CreateConnection())
                {
                    int rowsAffected = await connection.ExecuteAsync(updateQuery, parameters);
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }



    }
}
