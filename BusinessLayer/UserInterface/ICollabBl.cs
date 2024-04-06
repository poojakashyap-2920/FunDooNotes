using RepositoryLayer.Entity;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace BusinessLayer.UserInterface
{
    public interface ICollabBl
    {
        public Task<bool> AddCollaborator(int collabnoteid, string useremail, int collaborationId);


        public Task<IEnumerable<Collaboration>> GetAllCollaborators();
        public Task<bool> RemoveCollaborator(int noteid, string emailid);

    }
}
