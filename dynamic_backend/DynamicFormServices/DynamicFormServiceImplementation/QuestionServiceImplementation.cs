using DynamicFormPresentation.Models;
using DynamicFormRepos.DynamicFormRepoInterface;
using DynamicFormServices.Dto;
using DynamicFormServices.DynamicFormServiceInterface;
using Microsoft.EntityFrameworkCore;

namespace DynamicFormServices.DynamicFormServiceImplementation
{
    public class QuestionServiceImplementation : IQuestionServiceInterface
    {
        private readonly IQuestionRepoInterface _questionRepoInterface;
        private readonly SDirectContext _context;
        public QuestionServiceImplementation(IQuestionRepoInterface questionRepoInterface, SDirectContext context)
        {
            _questionRepoInterface = questionRepoInterface;
            _context = context;
        }

        public async Task<QuestionBank> CreateQuestionAsync(QuestionDto dto)
        {
            
            var question = new QuestionBank
            {
                Questions = dto.Question,
                Slno = dto.SerialNumber,
                DataType = dto.DataType,
                Constraints = dto.Constraint,
                ConstraintValue = dto.ConstraintValue,
                WarningMessage = dto.WarningMessage,
                CreatedOn = DateTime.UtcNow,
                Required=dto.Required,
                userId=dto.UserId,
            };

            await _questionRepoInterface.AddQuestionAsync(question);

            // Add answer options and map them to the question using AnswerMaster
            foreach (var optionDto in dto.AnswerOptions)
            {
                var answerOption = new AnswerOption
                {
                    OptionValue = optionDto.OptionValue,
                    AnswerTypeId = int.Parse(dto.ResponseType), // Assuming ResponseType is an integer ID
                    CreatedOn = DateTime.UtcNow
                };

                await _questionRepoInterface.AddAnswerOptionAsync(answerOption);

                var answerMaster = new AnswerMaster
                {
                    QuestionId = question.Id,
                    AnswerOptionId = answerOption.Id,
                    NextQuestionId = optionDto.NextQuestionId // Use the specific NextQuestionId for this option
                };

                await _questionRepoInterface.AddAnswerMasterAsync(answerMaster);
            }

            return question;
        }

        public async Task<QuestionDto> GetQuestionByIdAsync(int id)
        {
            var question = await _questionRepoInterface.GetQuestionByIdAsync(id);

            if (question == null)
                return null;

            // Safe access to the AnswerType property
            var answerOption = question.AnswerMasters
                .Select(am => am.AnswerOption)
                .FirstOrDefault(ao => ao != null);

            var answerType = answerOption?.AnswerType?.TypeName ?? string.Empty;

            var res = new QuestionDto
            {
                Id=question.Id,
                Question = question.Questions,
                SerialNumber = question.Slno ?? 0,
                ResponseType = answerType,
                AnswerOptions = question.AnswerMasters
                .Select(am => new AnswerOptionDto
                {
                    OptionValue = am.AnswerOption?.OptionValue,
                    NextQuestionId = am.NextQuestionId
                })
                .ToList()
            ,
               
               Required=question.Required,
                DataType = question.DataType,
                Constraint = question.Constraints,
                ConstraintValue = question.ConstraintValue,
                WarningMessage = question.WarningMessage
            };

            return res;
        }

        public async Task<bool> UpdateQuestionAsync(int id, QuestionDto dto)
        {
            var question = await _questionRepoInterface.GetQuestionByIdAsync(id);

            if (question == null)
                return false;

            question.Questions = dto.Question;
            question.Slno = dto.SerialNumber;
            question.DataType = dto.DataType;
            question.Constraints = dto.Constraint;
            question.ConstraintValue = dto.ConstraintValue;
            question.WarningMessage = dto.WarningMessage;
            question.Required = dto.Required;
           
            var existingAnswerMasters = question.AnswerMasters.ToList();
            _context.AnswerMasters.RemoveRange(existingAnswerMasters);

            
            foreach (var optionValue in dto.AnswerOptions)
            {
                var answerOption = await _questionRepoInterface.GetAnswerOptionByValueAsync(optionValue.OptionValue);

                if (answerOption != null)
                {
                    answerOption = new AnswerOption
                    {
                        OptionValue = optionValue.OptionValue,
                        AnswerTypeId = int.Parse(dto.ResponseType), 
                        CreatedOn = DateTime.UtcNow
                    };
                    await _questionRepoInterface.UpdateAnswerOptionAsync(answerOption);
                }

                var answerMaster = new AnswerMaster
                {
                    QuestionId = question.Id,
                    AnswerOptionId = answerOption.Id,
                    NextQuestionId = optionValue.NextQuestionId
                };

                await _questionRepoInterface.AddAnswerMasterAsync(answerMaster);
            }

            // Save changes
            await _questionRepoInterface.UpdateQuestionAsync(question);
            return true;
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            return await _questionRepoInterface.DeleteQuestionAsync(id);
        }



        public async Task<bool> MapQuestionToSectionAsync(int sectionId, int questionId)
        {
            return await _questionRepoInterface.AddQuestionToSectionAsync(sectionId, questionId);
        }

        public async Task<bool> DeleteMapQuestionToSectionAsync(int sectionId, int questionId)
        {
            // Fetch the mapping record from the database
            var mapping = await _context.SectionQuestionMappings
                .FirstOrDefaultAsync(m => m.SectionId == sectionId && m.QuestionId == questionId);

            if (mapping == null)
            {
                return false; // Mapping not found
            }

            // Remove the mapping record
            _context.SectionQuestionMappings.Remove(mapping);

            // Save changes
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<QuestionDto>> GetAllQuestionsAsync(int userId)
        {
            var questions = await _questionRepoInterface.GetAllQuestionAsync(userId);

            return questions.Select(question => new QuestionDto
            {
                Id = question.Id,
                Question = question.Questions,
                SerialNumber = question.Slno ?? 0,
                ResponseType = question.AnswerMasters
                    .Select(am => am.AnswerOption?.AnswerType?.TypeName)
                    .FirstOrDefault() ?? string.Empty,
                AnswerOptions = question.AnswerMasters
                    .Select(am => new AnswerOptionDto
                    {
                        OptionValue = am.AnswerOption?.OptionValue,
                        NextQuestionId = am.NextQuestionId
                    })
                        .ToList(),
                DataType = question.DataType,
                Constraint = question.Constraints,
                ConstraintValue = question.ConstraintValue,
                WarningMessage = question.WarningMessage
            }).ToList();
        }


    }
}
