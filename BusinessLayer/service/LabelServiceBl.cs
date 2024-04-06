using BusinessLayer.UserInterface;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.IUserEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.service
{
    public class LabelServiceBl : ILabelBl
    {
        private readonly ILabel _label;

        public LabelServiceBl(ILabel label)
        {
            _label = label; // Assign the parameter to the field, not itself
        }

        public Task<bool> AddLabelName(int collabnoteid, string useremail, string labelname)
        {
            return _label.AddLabelName(collabnoteid, useremail, labelname);
        }

        public async Task<bool> CheckQuery(int collabnoteid, string useremail, string labelname)
        {
            // Implement the logic to check if the label already exists
            // This logic should query the database to perform the check
            return await _label.CheckQuery(collabnoteid, useremail, labelname);
        }

        public Task<IEnumerable<LabelEntity>> GetAllLabel()
        {
            return _label.GetAllLabel();
        }

        public Task<bool> RemoveLabelName(int collabnoteid, string useremail)
        {
            return _label.RemoveLabelName(collabnoteid, useremail);
        }

//update

        Task<bool> ILabelBl.UpdateLabel(int collabnoteid, string useremail, string labelname)
        {
            return _label.UpdateLabel(collabnoteid,useremail, labelname);
           
        }
    }
}
