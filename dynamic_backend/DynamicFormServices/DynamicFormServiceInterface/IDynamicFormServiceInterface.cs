using DynamicFormPresentation.Models;
using DynamicFormServices.Dto;

namespace DynamicFormService.DynamicFormServiceInterface
{
    public interface IDynamicFormServiceInterface
    {
        Task<IEnumerable<UserCredential>> GetAllUsersAsync();
        Task<IEnumerable<QuestionBank>> GetAllQuestion();

        Task<SectionDto> GetSectionByIdAsync(int id);
        Task<IEnumerable<SectionDto>> GetAllSectionsAsync();
        Task<FormDto> CreateForm(FormsTable form);
        Task<IEnumerable<FormDto>> GetAllFormsAsync(int userId);
        Task<FormDto> GetFormByIdAsync(int id);
        Task<FormDto> UpdateFormAsync(FormsTable form);
        Task<bool> DeleteFormAsync(int id);





        public Task<int> CreateSectionsAsync(SectionDto sectionDtos);
        Task<SectionDto> UpdateSectionAsync(SectionTable section);

        Task<bool> DeleteSectionAsync(int id);




        
    }
}
