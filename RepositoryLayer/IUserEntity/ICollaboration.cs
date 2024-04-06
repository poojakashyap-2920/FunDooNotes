using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.IUserEntity
{
public  interface ICollaboration
    {
        public Task<bool> AddCollaborator(int collabnoteid, string useremail, int collaborationId);


        public Task<IEnumerable<Entity.Collaboration>> GetAllCollaborators();
        public Task<bool> RemoveCollaborator(int collabnoteid, string useremail);
    }
}
