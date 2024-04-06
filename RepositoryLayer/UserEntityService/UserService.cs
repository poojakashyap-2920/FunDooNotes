using Dapper;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Exceptions;
using RepositoryLayer.IUserEntity;
using RepositoryLayer.Session;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Session;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Web.Http.Results;

namespace RepositoryLayer.UserEntityService
{
    public class UserService : IUser
    {

        private readonly UserContext _context;
        private readonly UserEntity _user;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //logging 

        public readonly ILogger<UserService> logger;
       

        //session
        //    private readonly SessionManager _sessionManag;

        private static string otp;
        private static string mailid;
        private static UserEntity entity;


        public UserService(UserContext context, UserEntity user, ILogger<UserService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = user;
            this.logger = logger;
            _httpContextAccessor = httpContextAccessor;
            // sessionManag= sessionManag;
        }

        // insert
        public  async Task InsertUser(string firstname, string lastname, string emailid, string password)
        {
            var query = "insert into userentity(FirstName,LastName,EmailId,Password) values(@firstname,@lastname,@emailid,@password)";
            string encryptedPassword = EncyptionPassword(password);
            //await Console.Out.WriteLineAsync(encryptedPassword);
            var Parameter = new DynamicParameters();
            Parameter.Add("firstname", firstname, DbType.String);
            Parameter.Add("lastname", lastname, DbType.String);
            Parameter.Add("emailid", emailid, DbType.String);
            Parameter.Add("password", encryptedPassword, DbType.String);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, Parameter);

            }
        }


        // Encryption
        private string EncyptionPassword(string password)
        {
            byte[] plaintext = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(plaintext);
        }


        //decryption
        public string DecryptPassword(string encryptedPassword)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);
            return Encoding.UTF8.GetString(encryptedBytes);
        }


        //delete
        //delete
        public async Task DeleteEmployeeByEmail(string emailid)
        {

                var query = "DELETE FROM userentity WHERE emailid = @EmailId";
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new { EmailId = emailid });
                }
            
            
        }


        //get result based on email
        public async Task<IEnumerable<UserEntity>> GetEmployeeByEmail(string emailid)
        {
            var query = "SELECT * FROM userentity WHERE emailid = @emailId";
            using (var connection = _context.CreateConnection())
            {
                var employee = await connection.QueryAsync<UserEntity>(query, new { emailId = emailid });
                return employee.ToList();
            }
        }

        //getlastname
        public  async Task<IEnumerable<UserEntity>> GetLastName()
        {
            var query = "select  lastname from userentity";
            using (var connection = _context.CreateConnection())
            {
                var employee = await connection.QueryAsync<UserEntity>(query);
                return employee.ToList();
            }
        }

        //get all result
        public async Task<IEnumerable<UserEntity>> GetUser()
        {
           var query = "SELECT * FROM userentity";
            using (var connection = _context.CreateConnection())
            {
                var employee = await connection.QueryAsync<UserEntity>(query);
               /* logger.LogInformation("all values show successfully...");
                logger.LogWarning("no need to pass email...");
                logger.LogDebug("debug the code debuge...");*/
                logger.LogTrace("Doing something very detailed and verbose...");

                // Simulate some debug information
                logger.LogDebug("Debugging information...");

                // Simulate some information message
                logger.LogInformation("Information message...");

                // Simulate a warning
                logger.LogWarning("Warning message...");

                // Simulate an error
                logger.LogError("Error message...");

                // Simulate a critical error
                logger.LogCritical("Critical error message...");

                return employee.ToList();
            }
        }



        //login
        public async Task<IEnumerable<UserEntity>> Login(string emailid, string password)
        {
            var query = "SELECT * FROM userentity WHERE EmailId = @EmailId";
            using (var connection = _context.CreateConnection())
            {
                var employee = await connection.QueryAsync<UserEntity>(query, new { EmailId = emailid });
                foreach (var item in employee)
                {
                    string storeredPassword = DecryptPassword(item.Password);
                    if (password == storeredPassword)
                    {
                        return new List<UserEntity> { item };
                    }
                }
                return Enumerable.Empty<UserEntity>();
            }
        }

        public string GetSessionToken()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("Token");
        }

        //orderbyfirstname
        public  async Task<IEnumerable<UserEntity>> OrederByFirstName()
        {
            var query = "select * from userentity order by firstname";
            using (var connection = _context.CreateConnection())
            {
                var employee = await connection.QueryAsync<UserEntity>(query);
                return employee.ToList();
            }
        }



        // updateuserbyemail
        public async Task UpdateUserByEmail(string emailid, string firstname, string lastname)
        {
            var query = "UPDATE userentity SET FirstName = @FirstName, LastName = @LastName WHERE EmailId = @EmailId";

            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", firstname, DbType.String);
            parameters.Add("@LastName", lastname, DbType.String);
            parameters.Add("@EmailId", emailid, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }


        //resetpasswordbyemail
        public async Task<int> ResetPasswordByEmail(string emailid, string password)
        {
            var query = "UPDATE userentity SET Password = @password WHERE EmailId = @Email";

            var parameters = new DynamicParameters();
            parameters.Add("@password", password, DbType.String);
            parameters.Add("@Email", emailid, DbType.String);
            int rowsAffected = 0;
            using (var connection = _context.CreateConnection())
            {

                return await connection.ExecuteAsync(query, parameters);


            }
        }



        //changePassword
        public Task<string> ChangePassword(string otp, string password)
        {
            if (otp.Equals(null))
            {
                return Task.FromResult("Generate Otp First");
            }
            if (DecryptPassword(entity.Password).Equals(password))
            {
                throw new PasswordMissmatchException("Dont give the existing password");
            }

            if (/*Regex.IsMatch(password, @"^(?=.[a-z])(?=.[A-Z])(?=.\d)(?=.[!@#$%^&])[a-zA-Z\d!@#$%^&]{8,16}$")*/true)
            {
                if (otp.Equals(otp))
                {
                    if (ResetPasswordByEmail(mailid, EncyptionPassword(password)).Result == 1)
                    {
                        entity = null; otp = null; mailid = null;
                        return Task.FromResult("password changed successfully");
                    }
                }
                else
                {
                    return Task.FromResult("otp miss matching");
                }
            }
            else
            {
                return Task.FromResult("regex is mismatching");
            }
            return Task.FromResult("password not changed");
        }



        //changepasswordrequest
        public async Task<string> ChangePasswordRequest(string Email)
        {
            try
            {
                // Corrected asynchronous call
                var employees = await GetEmployeeByEmail(Email);
                entity = employees.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new UserNotFoundException("UserNotFoundByEmailId", e.StackTrace);
            }

            string generatedotp = "";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                generatedotp += r.Next(0, 10);
            }
            otp = generatedotp;
            mailid = Email;
            sendMail(Email, generatedotp);
            Console.WriteLine(otp);
            return "MailSent ✔";
        }



        //send mail
        private static void sendMail(String ToMail, String otp)
        {
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            try
            {
                mailMessage.From = new System.Net.Mail.MailAddress("m_raghu@outlook.com", "FUNDOO NOTES");
                mailMessage.To.Add(ToMail);
                mailMessage.Subject = "Change password for Fundoo Notes";
                mailMessage.Body = "This is your otp please enter to change password " + otp;
                mailMessage.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp-mail.outlook.com");

                // Specifies how email messages are delivered. Here Email is sent through the network to an SMTP server.
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                // Set the port for Outlook's SMTP server
                smtpClient.Port = 587; // Outlook SMTP port for TLS/STARTTLS

                // Enable SSL/TLS
                smtpClient.EnableSsl = true;

                string loginName = "m_raghu@outlook.com";
                string loginPassword = "R@ghu2k01";

                System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential(loginName, loginPassword);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = networkCredential;

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught: " + ex.Message);
            }
            finally
            {
                mailMessage.Dispose();
            }
        }

        public static async Task<int> GetEmployeeByEmail(int emailId)
        {
            throw new NotImplementedException();
        }






        // Logout method
        /* public void Logout(string emailid)
         {
             _sessionManag.RemoveSession(emailid);
         }

         //session login

         public async Task<UserEntity> Login(string emailid, string password)
         {
             var query = "SELECT * FROM userentity WHERE EmailId = @EmailId";
             using (var connection = _context.CreateConnection())
             {
                 var user = await connection.QueryFirstOrDefaultAsync<UserEntity>(query, new { EmailId = emailid });
                 if (user != null)
                 {
                     string storedPassword = DecryptPassword(user.Password);



                     if (password == storedPassword)
                     {
                         // Create session
                         string sessionId = _sessionManag.CreateSession(emailid);
                         user.SessionId = sessionId;
                         return user;
                     }
                 }
                 return null;
             }
         }*/

        /* public async  IEnumerable<UserEntity> Login(string emailid, string password)
         {
             var query = "SELECT * FROM userentity WHERE EmailId = @EmailId";
             using (var connection = _context.CreateConnection())
             {
                 var user = await connection.QueryFirstOrDefaultAsync<UserEntity>(query, new { EmailId = emailid });
                 if (user != null)
                 {
                     string storedPassword = DecryptPassword(user.Password);
                     if (password == storedPassword)
                     {
                         // Create session
                         string sessionId = _sessionManag.CreateSession(emailid);
                         user.SessionId = sessionId;
                         return user;
                     }
                 }
                 return null;
             }
         }*/
    }
}

