using BusinessLayer.UserInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using System;
using System.Threading.Tasks;

namespace FunDooN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaborationController : ControllerBase
    {
        private readonly ICollabBl _collabBl;

        public CollaborationController(ICollabBl collabBl)
        {
            _collabBl = collabBl;
        }

        // Insert collaboration
        [HttpPost]
        public async Task<IActionResult> AddCollaborator([FromBody] Collaboration cbDto)
        {
            try
            {
                if (cbDto == null)
                {
                    return BadRequest("Invalid request. Please provide the required data.");
                }

                if (string.IsNullOrWhiteSpace(cbDto.useremail))
                {
                    return BadRequest("User email is required.");
                }

                bool isInserted = await _collabBl.AddCollaborator(cbDto.collabnoteid, cbDto.useremail, cbDto.collaborationid);

                if (isInserted)
                {
                    return Ok("Collaborator added successfully");
                }
                else
                {
                    return Conflict("Collaborator already exists");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the collaborator: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCollaborators()
        {
            try
            {
                var collaborators = await _collabBl.GetAllCollaborators();
                return Ok(collaborators);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving collaborators: " + ex.Message);
            }
        }

        // DELETE: /collaboration/{collabnoteid}/{useremail}
        [HttpDelete]
        public async Task<IActionResult> RemoveCollaborator(int collabnoteid, string useremail)
        {
            try
            {
                bool success = await _collabBl.RemoveCollaborator(collabnoteid, useremail);

                if (success)
                {
                    return Ok("Record deleted successfully");
                }
                else
                {
                    return NotFound("Note ID and user email not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while removing the collaborator: " + ex.Message);
            }
        }
    }
}
