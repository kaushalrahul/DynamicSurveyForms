using DynamicFormPresentation.Models;

namespace DynamicFormRepos.DynamicFormRepoInterface
{
    public interface IAnswerTypeRepository
    {

        Task<IEnumerable<AnswerType>> GetAllAsync();
        Task<AnswerType?> GetByIdAsync(int id);

    }
}
