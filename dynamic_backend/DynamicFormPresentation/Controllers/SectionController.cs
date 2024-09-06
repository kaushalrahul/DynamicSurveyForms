using DynamicFormPresentation.Models;
using DynamicFormService.DynamicFormServiceInterface;
using DynamicFormServices.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DynamicFormPresentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly IDynamicFormServiceInterface _dynamicFormServiceInterface;

        public SectionController(IDynamicFormServiceInterface dynamicFormServiceInterface)
        {
            _dynamicFormServiceInterface = dynamicFormServiceInterface;
        }
       



        [HttpGet("GetAllSection")]
        public async Task<IActionResult> GetAllSections()
        {
            var sections = await _dynamicFormServiceInterface.GetAllSectionsAsync();
            return Ok(sections);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSectionById(int id)
        {
            var section = await _dynamicFormServiceInterface.GetSectionByIdAsync(id);
            if (section == null)
            {
                return NotFound();
            }
            return Ok(section);
        }









        [HttpPost("CreateSection")]
        public async Task<IActionResult> CreateSection([FromBody] SectionDto sectionDtos)
        {
            if (!ModelState.IsValid || sectionDtos == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var res = await _dynamicFormServiceInterface.CreateSectionsAsync(sectionDtos);
                return Ok(new {sectionId = res}); // Return an appropriate response, e.g., created sections or success message
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("UpdateSection/{id}")]
        public async Task<IActionResult> UpdateSection(int id, [FromBody] SectionDto sectionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var section = new SectionTable
            {
                Id = id,
                Active= true,
                FormId = sectionDto.FormId,
                SectionName = sectionDto.SectionName,
                Description = sectionDto.Description,
                Slno = sectionDto.Slno,
                CreatedOn = DateTime.UtcNow,
            };

            var updatedSection = await _dynamicFormServiceInterface.UpdateSectionAsync(section);
            if (updatedSection == null)
            {
                return NotFound();
            }

            return Ok(updatedSection);
        }

        [HttpDelete("DeleteSection/{id}")]
        public async Task<IActionResult> DeleteSection(int id)
        {
            var result = await _dynamicFormServiceInterface.DeleteSectionAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }



        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _dynamicFormServiceInterface.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
               
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


    }
}
