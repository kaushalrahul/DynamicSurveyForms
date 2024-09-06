using DynamicFormPresentation.Models;
using DynamicFormServices.Dto;

namespace DynamicFormServices.DynamicFormServiceInterface
{
    public interface IQuestionServiceInterface
    {

        Task<QuestionBank> CreateQuestionAsync(QuestionDto dto);

        Task<QuestionDto> GetQuestionByIdAsync(int id);

        Task<bool> UpdateQuestionAsync(int id, QuestionDto dto);

        Task<bool> DeleteQuestionAsync(int id);

        Task<bool> MapQuestionToSectionAsync(int sectionId, int questionId);

        Task<bool> DeleteMapQuestionToSectionAsync(int sectionId, int questionId);


        Task<List<QuestionDto>> GetAllQuestionsAsync(int userId);


    }
}
