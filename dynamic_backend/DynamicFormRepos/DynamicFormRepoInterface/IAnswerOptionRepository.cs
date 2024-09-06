using DynamicFormPresentation.Models;

namespace DynamicFormRepos.DynamicFormRepoInterface
{
    public interface IAnswerOptionRepository
    {
        Task<AnswerOption?> GetByIdAsync(int id);
    }
}
