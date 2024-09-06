using DynamicFormServices.Dto;
using DynamicFormServices.DynamicFormServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DynamicFormPresentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {

        private readonly IQuestionServiceInterface _questionServiceInterface;

        public QuestionController(IQuestionServiceInterface questionServiceInterface)
        {
            _questionServiceInterface = questionServiceInterface;
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllQuestions()
        {
            int userId = int.Parse(User.FindFirst("UserID").Value);

            

            var questions = await _questionServiceInterface.GetAllQuestionsAsync(userId);

            if (questions == null || questions.Count == 0)
            {
                return NotFound("No questions found.");
            }

            return Ok(questions);
        }



        [HttpPost("CreateQuestion")]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdQuestion = await _questionServiceInterface.CreateQuestionAsync(dto);

            if (createdQuestion == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the question.");
            }

            return CreatedAtAction(nameof(CreateQuestion), new { id = createdQuestion.Id }, createdQuestion);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestionById(int id)
        {
            var questionDto = await _questionServiceInterface.GetQuestionByIdAsync(id);

            if (questionDto == null)
                return NotFound();

            return Ok(questionDto);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] QuestionDto dto)
        {
            

            var success = await _questionServiceInterface.UpdateQuestionAsync(id, dto);

            if (success)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                return NotFound(); // 404 Not Found
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var success = await _questionServiceInterface.DeleteQuestionAsync(id);

            if (success)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                return NotFound(); // 404 Not Found
            }
        }



        [HttpPost("MapQuestionToSection")]
        public async Task<IActionResult> MapQuestionToSection([FromBody] SectionQuestionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _questionServiceInterface.MapQuestionToSectionAsync(dto.SectionId, dto.QuestionId);

            if (success)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                /* return Conflict("The question is already mapped to this section.");*/ // 409 Conflict

                return Ok("The question is already mapped to this section.");
            }
        }

        [HttpDelete("MapQuestionToSection/{sectionId}/{questionId}")]
        public async Task<IActionResult> DeleteMapQuestionToSection(int sectionId, int questionId)
        {
            var success = await _questionServiceInterface.DeleteMapQuestionToSectionAsync(sectionId, questionId);

            if (success)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                return NotFound(); // 404 Not Found
            }
        }


    }
}
