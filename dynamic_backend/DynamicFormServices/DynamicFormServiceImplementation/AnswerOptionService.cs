using DynamicFormPresentation.Models;
using DynamicFormRepos.DynamicFormRepoInterface;
using DynamicFormServices.DynamicFormServiceInterface;

namespace DynamicFormServices.DynamicFormServiceImplementation
{
    public class AnswerOptionService : IAnswerOptionService
    {
        private readonly IAnswerOptionRepository _repository;

        public AnswerOptionService(IAnswerOptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<AnswerOption?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
