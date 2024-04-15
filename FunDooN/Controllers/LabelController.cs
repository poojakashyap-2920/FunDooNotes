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
    public class LabelController : ControllerBase
    {
        private readonly ILabelBl _labelBl;

        public LabelController(ILabelBl labelBl)
        {
            _labelBl = labelBl;
        }

        // Insert label
        [HttpPost]
        public async Task<IActionResult> AddLabelName([FromBody] LabelEntity cbDto)
        {
            try
            {
                // Validate input
                if (cbDto == null || string.IsNullOrWhiteSpace(cbDto.useremail) || string.IsNullOrWhiteSpace(cbDto.LabelName))
                {
                    return BadRequest("Invalid request. Please provide the required data.");
                }

                // Check if the label already exists for the user
                bool labelExists = await _labelBl.CheckQuery(cbDto.collabnoteid, cbDto.useremail, cbDto.LabelName);

                if (labelExists)
                {
                    return Conflict("Label name already exists");
                }

                // If the label doesn't exist, add it
                bool isInserted = await _labelBl.AddLabelName(cbDto.collabnoteid, cbDto.useremail, cbDto.LabelName);

                if (isInserted)
                {
                    return Ok("Label added successfully");
                }
                else
                {
                    return StatusCode(500, "Failed to add label. Please try again later.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the label: " + ex.Message);
            }
        }


        // DELETE: /label/{collabnoteid}/{useremail}
        [HttpDelete]
        public async Task<IActionResult> RemoveLabelName(int collabnoteid, string useremail)
        {
            try
            {
                bool success = await _labelBl.RemoveLabelName(collabnoteid, useremail);

                if (success)
                {
                    return Ok("Label deleted successfully");
                }
                else
                {
                    return NotFound("Label not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while removing the label: " + ex.Message);
            }
        }

        // GET: /label/getalllabel
        [HttpGet]
        public async Task<IActionResult> GetAllLabel()
        {
            try
            {
                var labels = await _labelBl.GetAllLabel();
                return Ok(labels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving labels: " + ex.Message);
            }
        }


        // Update label
        [HttpPut]
        public async Task<IActionResult> UpdateLabel(int collabnoteid, string useremail, string LabelName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LabelName))
                {
                    return BadRequest("Label name cannot be empty.");
                }

               bool isUpdated = await _labelBl.UpdateLabel(collabnoteid, useremail, LabelName);

                if (isUpdated)
                {
                    return Ok("Label updated successfully");
                }
                else
                {
                    return NotFound("Label not found or failed to update.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the label: " + ex.Message);
            }
        }


    }
}
