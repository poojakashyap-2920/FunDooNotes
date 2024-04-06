using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.UserInterface 
{
    public interface IUserBl
    {
        //insert

        public Task InsertUser(string firstname, string lastname, string emailid, string password);

       
        //get all record
        public Task<IEnumerable<UserEntity>> GetUser();

        //update
        public Task UpdateUserByEmail(string firstname, string lastname, string emailid);

        //delete
        public Task DeleteEmployeeByEmail(string emailid);

        //get all record by email
        public Task<IEnumerable<UserEntity>> GetEmployeeByEmail(string emailid);


        //login
        public Task<IEnumerable<UserEntity>> Login(string emailid, string password);


        // Review 
        public Task<IEnumerable<UserEntity>> GetLastName();

        //order by name
        public Task<IEnumerable<UserEntity>> OrederByFirstName();




        //  public Task CheckUpdateName(string firstname, string lastname, string emailid, string password);


        //*************************
        public Task<String> ChangePasswordRequest(string Email);
        public Task<string> ChangePassword(string otp, string password);
        //Reset Password
        public Task<int> ResetPasswordByEmail(string emailid, string newPassword);

    }
}
