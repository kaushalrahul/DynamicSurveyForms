using DynamicFormServices.DynamicFormServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace DynamicFormPresentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerOptionController : ControllerBase
    {
        private readonly IAnswerOptionService _service;

        public AnswerOptionController(IAnswerOptionService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var answerOption = await _service.GetByIdAsync(id);
            if (answerOption == null)
            {
                return NotFound();
            }
            return Ok(answerOption);
        }
    }
}
