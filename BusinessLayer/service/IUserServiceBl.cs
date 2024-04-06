using BusinessLayer.UserInterface;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.IUserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.service
{
    public class IUserServiceBl : IUserBl
    {
        private readonly IUser _user;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public IUserServiceBl(IUser user, IHttpContextAccessor httpContextAccessor)
        {
            _user = user;
            _httpContextAccessor = httpContextAccessor;

        }

      

        //delete
        public Task DeleteEmployeeByEmail(string emailid)
        {
            return _user.DeleteEmployeeByEmail(emailid);
        }

        //getbyid

        public Task<IEnumerable<UserEntity>> GetEmployeeByEmail(string emailid)
        {
            return _user.GetEmployeeByEmail(emailid);
        }

        //getlastname
        public Task<IEnumerable<UserEntity>> GetLastName()
        {
           return _user.GetLastName();
        }

        //getall
        public Task<IEnumerable<UserEntity>> GetUser()
        {
            return _user.GetUser();
        }

        // insert
        public Task InsertUser(string firstname, string lastname, string emailid, string password)
        {
            return _user.InsertUser(firstname, lastname, emailid, password);
        }

        //login
        public Task<IEnumerable<UserEntity>> Login(string emailid, string password)
        {
            return _user.Login(emailid, password);
        }

        //session management
        public string GetSessionToken()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("Token");
        }

        // Set session token
        public void SetSessionToken(string token)
        {
            _httpContextAccessor.HttpContext.Session.SetString("Token", token);
        }

        // Remove session token
        public void RemoveSessionToken()
        {
            _httpContextAccessor.HttpContext.Session.Remove("Token");
        }


        //orderbyname
        public Task<IEnumerable<UserEntity>> OrederByFirstName()
        {
            return _user.OrederByFirstName();
        }

        

        //update
        public Task UpdateUserByEmail(string firstname, string lastname, string emailid)
        {
            return _user.UpdateUserByEmail(firstname, lastname, emailid);
        }


        //resetpasswordbyemail

        public Task<int> ResetPasswordByEmail(string emailid, string newPassword)
        {
            return _user.ResetPasswordByEmail(emailid, newPassword);
        }


        //changepassword
        public Task<string> ChangePassword(string otp, string password)
        {
           return _user.ChangePassword(otp, password);
        }

        //changepasswordrequest
        public Task<string> ChangePasswordRequest(string Email)
        {
            return _user.ChangePasswordRequest(Email);
        }
    }
}


