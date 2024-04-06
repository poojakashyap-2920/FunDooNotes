using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.UserInterface
{
   public  interface ILabelBl
    {
        Task<bool> AddLabelName(int collabnoteid, string useremail, string labelname);
        Task<bool> CheckQuery(int collabnoteid, string useremail, string labelname);


        public Task<IEnumerable<LabelEntity>> GetAllLabel();
        public Task<bool> RemoveLabelName(int collabnoteid, string useremail);
        Task<bool> UpdateLabel(int collabnoteid, string useremail, string labelname);

    }
}
