using DynamicFormPresentation.Models;
using DynamicFormServices.DynamicFormServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamicFormPresentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerTypeController : ControllerBase
    {


        private readonly IAnswerTypeService _service;

        public AnswerTypeController(IAnswerTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerType>>> GetAll()
        {
            var answerTypes = await _service.GetAllAsync();
            return Ok(answerTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerType>> GetById(int id)
        {
            var answerType = await _service.GetByIdAsync(id);
            if (answerType == null)
            {
                return NotFound();
            }
            return Ok(answerType);
        }
    }
}
