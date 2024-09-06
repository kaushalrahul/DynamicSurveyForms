using DynamicFormPresentation.Models;
using DynamicFormRepos.DynamicFormRepoInterface;
using DynamicFormServices.DynamicFormServiceInterface;

namespace DynamicFormServices.DynamicFormServiceImplementation
{
    public class AnswerTypeService:IAnswerTypeService
    {
        private readonly IAnswerTypeRepository _repository;

        public AnswerTypeService(IAnswerTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AnswerType>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }


        public async Task<AnswerType?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
