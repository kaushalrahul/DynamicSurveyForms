using DynamicFormPresentation.Models;

namespace DynamicFormRepo.DynamicFormRepoInterface
{
    public interface IDynamicFormRepoInterface
    {
        Task<IEnumerable<UserCredential>> GetAllAsync();

        Task<IEnumerable<QuestionBank>> GetAllQuestion();
        Task<IEnumerable<SectionTable>> GetAllSectionAsync();

        Task<FormsTable> CreateForm(FormsTable form);


        Task<IEnumerable<FormsTable>> GetAllFormsAsync(int userId);
        Task<FormsTable> GetFormByIdAsync(int id);
        Task<FormsTable> UpdateFormAsync(FormsTable form);
        Task<bool> DeleteFormAsync(int id);


        Task<SectionTable> GetBySectionIdAsync(int id);

        Task<SectionTable> CreateSectionAsync(SectionTable section);

        Task<SectionTable> UpdateSectionAsync(SectionTable section);

        Task<bool> DeleteSectionAsync(int id);

     
    }
}
