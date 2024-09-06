using DynamicFormPresentation.Models;
using DynamicFormService.DynamicFormServiceInterface;
using DynamicFormServices.Dto;
using DynamicFormServices.DynamicFormServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicFormPresentation.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        private readonly IDynamicFormServiceInterface _formServiceInterface;
        private readonly IFormService _formInterface;



        public FormController(IDynamicFormServiceInterface formServiceInterface, IFormService formInterface)
        {
            _formServiceInterface = formServiceInterface;
            _formInterface = formInterface;
        }


        [HttpPost("CreateForm")]
        public async Task<IActionResult> CreateForm([FromBody] FormDto formDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var form = new FormsTable
            {
                UserId = formDto.UserId,
                FormName = formDto.FormName,
                Comments = formDto.Description,
                IsPublish = formDto.IsPublish,
                Version = formDto.Version,
                CreatedOn=DateTime.Now,
                CreatedByUserId=formDto.UserId,
            };

            var result = await _formServiceInterface.CreateForm(form);
            
            if (result == null) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the form.");
            }

            return StatusCode(StatusCodes.Status201Created, result.Id);
        }


        [Authorize]
        [HttpGet("GetAllForms")]
        public async Task<IActionResult> GetAllForms()
        {
            // Extract the user ID from the JWT token
            int userId = int.Parse(User.FindFirst("UserID").Value);

            // Pass the user ID to the service layer to filter forms
            var forms = await _formServiceInterface.GetAllFormsAsync(userId);
            return Ok(forms);
        }


        [HttpGet("GetFormById/{id}")]
        public async Task<IActionResult> GetFormById(int id)
        {
            var form = await _formServiceInterface.GetFormByIdAsync(id);
            if (form == null)
            {
                return NotFound(new {Success = false});
            }
            return Ok(form);
        }

        [HttpPut("UpdateForm/{id}")]
        public async Task<IActionResult> UpdateForm(int id, [FromBody] FormDto formDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var form = new FormsTable
            {
                Id = id,
                UserId = formDto.UserId,
                FormName = formDto.FormName,
                Comments = formDto.Description,
                IsPublish=formDto.IsPublish,
                Version = formDto.Version + 1,
                ModifiedOn=DateTime.Now,
                ModifiedByUserId=formDto.UserId,

            };

            var updatedForm = await _formServiceInterface.UpdateFormAsync(form);
            if (updatedForm == null)
            {
                return NotFound();
            }

            return Ok(updatedForm);
        }

        [HttpDelete("DeleteForm/{id}")]
        public async Task<IActionResult> DeleteForm(int id)
        {
            var result = await _formServiceInterface.DeleteFormAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpGet("GetFormDetails/{formId}")]
        public async Task<IActionResult> GetFormDetails(int formId)
        {
            var formDetails = await _formInterface.GetFormDetailsByFormIdAsync(formId);

            if (formDetails == null)
            {
                return NotFound();
            }

            return Ok(formDetails);
        }



        [HttpGet("GetFormById_nextQuestion/{formId}")]
        public async Task<IActionResult> GetFormById_nextQuestion(int formId)
        {
            try
            {
                var res = await _formInterface.GetFormById_next_question(formId);

                if (res != null)
                {
                    return Ok(new { success = true, form = res });

                }
                return StatusCode(500, new { success = false, message = "not found" });

            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = "internalServerError, error = ex.Message" });
            }
        }

    }
}
