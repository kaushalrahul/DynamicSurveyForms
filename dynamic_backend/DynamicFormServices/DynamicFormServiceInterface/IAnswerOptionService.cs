using DynamicFormPresentation.Models;

namespace DynamicFormServices.DynamicFormServiceInterface
{
    public interface IAnswerOptionService
    {
        Task<AnswerOption?> GetByIdAsync(int id);
    }
}
