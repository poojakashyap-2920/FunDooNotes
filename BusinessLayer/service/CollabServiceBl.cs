using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.UserInterface; // Assuming correct namespace
using RepositoryLayer.Entity;
using RepositoryLayer.IUserEntity;

namespace BusinessLayer.service
{
    public class CollabServiceBl : ICollabBl
    {
        private readonly ICollaboration collab;

        public CollabServiceBl(ICollaboration collab)
        {
            this.collab = collab; // Correctly assign the parameter to the field
        }

        public Task<bool> AddCollaborator(int noteid, string emailid, int collaborationId)
        {
            return collab.AddCollaborator(noteid, emailid, collaborationId);
        }

        public Task<IEnumerable<Collaboration>> GetAllCollaborators()
        {
            return collab.GetAllCollaborators();
        }

        public Task<bool> RemoveCollaborator(int collabnoteid, string useremail)
        {
            return collab.RemoveCollaborator(collabnoteid, useremail);
        }
    }
}
