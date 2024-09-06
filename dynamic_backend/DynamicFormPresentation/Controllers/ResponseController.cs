using DynamicFormServices.Dto;
using DynamicFormServices.DynamicFormServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace DynamicFormPresentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        private readonly IResponseFormServiceInterface _responseFormServiceInterface;

        public ResponseController(IResponseFormServiceInterface responseFormServiceInterface)
        {
            _responseFormServiceInterface = responseFormServiceInterface;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetResponseById(int id)
        {
            var responseDto = await _responseFormServiceInterface.GetResponseByIdAsync(id);
            if (responseDto == null)
            {
                return NotFound();
            }
            return Ok(responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddResponse([FromBody] ResponseDto responseDto)
        {
            if (responseDto == null)
                return BadRequest("Response DTO cannot be null");

            if (string.IsNullOrEmpty(responseDto.Response))
                return BadRequest("The response field is required.");

            // Log the incoming response for debugging
            Console.WriteLine("Received responseDto: " + responseDto.Response);

            await _responseFormServiceInterface.SubmitResponseAsync(responseDto);

            return CreatedAtAction(nameof(GetResponseById), new { id = responseDto.FormID }, responseDto);
        }

        [HttpGet("form/{formId}/responses")]
        public async Task<IActionResult> GetResponseIdsByFormId(int formId)
        {
            var responseIds = await _responseFormServiceInterface.GetResponseIdsByFormIdAsync(formId);
            if (responseIds == null || !responseIds.Any())
            {
                return NotFound();
            }

            return Ok(responseIds);
        }

        [HttpGet("form/{formId}/responses/count")]
        public async Task<IActionResult> GetResponseCountByFormId(int formId)
        {
            var count = await _responseFormServiceInterface.GetResponseCountByFormIdAsync(formId);
            return Ok(count);
        }
    }
}
