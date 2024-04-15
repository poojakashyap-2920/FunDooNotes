using BusinessLayer.UserInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace FunDooN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {

          private readonly ICobllabBl _noteBl;
            private readonly IConfiguration _configuration;
            public NoteController(ICobllabBl noteBl, IConfiguration configuration)
            {
                _noteBl = noteBl;
                _configuration = configuration;
            }


            //insert
            [HttpPost]
            public async Task<IActionResult> InsertNote([FromBody] NotesEntity noteDto, string emailid)
            {
                try
                {
                    // Call the NoteBl to insert the note
                    await _noteBl.InsertNote(noteDto.Title, noteDto.Description, noteDto.Colour, noteDto.IsArchived, noteDto.IsDeleted, emailid);

                    // Return a success response
                    return Ok("Note inserted successfully");
                }
                catch (Exception ex)
                {
                    // Return an error response if an exception occurs
                    return StatusCode(500, "An error occurred while inserting the note: " + ex.Message);
                }
            }

            // get all
            [HttpGet]
            public async Task<IActionResult> GetNote()
            {
                try
                {
                    var notes = await _noteBl.GetNote();
                    return Ok(notes);
                }
                catch (Exception ex)
                {
                    // Log error
                    return StatusCode(500, ex.Message);
                }
            }


            //get note  by noteid
            [HttpGet("GetNoteByNoteId")]
            public async Task<IActionResult> GetNotesById(int NoteId)
            {
                try
                {
                    var note = await _noteBl.GetNotesById(NoteId);
                    return Ok(note);
                }
                catch (Exception ex)
                {
                    // Log error
                    return StatusCode(500, ex.Message);
                }
            }
            //delete noteby id
            [HttpDelete] // Use the same parameter name as defined in the method signature
            public async Task<IActionResult> DeleteNoteById(int noteid)
            {
                // Call the service to delete the note by its ID
                var note = await _noteBl.GetNotesById(noteid);

                // Check if the note was successfully deleted
                if (note != null)
                {
                    // Call the method to delete the note from the database
                    await _noteBl.DeleteNoteById(noteid);
                    return Ok("Note deleted successfully.");
                }
                else
                {
                    return NotFound("Note not found with the provided ID.");
                }
            }

            [HttpPut] // Specify the noteid parameter in the route template
            public async Task<IActionResult> UpdateNoteByNoteId(int noteid, [FromBody] NotesEntity updateDto)
            {
                try
                {
                    if (updateDto == null)
                    {
                        return BadRequest("Invalid data provided");
                    }

                    // Call the business layer method to update the note
                    await _noteBl.UpdateNoteByNoteId(noteid, updateDto);

                    return Ok("Note updated successfully");
                }
                catch (Exception ex)
                {
                    // Log the exception
                    return StatusCode(500, $"An error occurred while updating the note: {ex.Message}");
                }
            }


        }
    }

