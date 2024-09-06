using DynamicFormServices.Dto;

namespace DynamicFormServices.DynamicFormServiceInterface
{
    public interface IFormService
    {
        Task<FormDetailsDto> GetFormDetailsByFormIdAsync(int formId);
        public Task<FetchFormDto> GetFormById_next_question(int formId);
    }
}






