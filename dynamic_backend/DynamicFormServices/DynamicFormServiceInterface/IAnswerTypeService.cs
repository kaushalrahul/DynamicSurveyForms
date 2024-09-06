using DynamicFormPresentation.Models;

namespace DynamicFormServices.DynamicFormServiceInterface
{
    public interface IAnswerTypeService
    {
        Task<IEnumerable<AnswerType>> GetAllAsync();
        Task<AnswerType?> GetByIdAsync(int id);
    }
}
