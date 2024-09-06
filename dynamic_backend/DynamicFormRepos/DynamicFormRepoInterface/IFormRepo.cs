using DynamicFormPresentation.Models;

namespace DynamicFormRepos.DynamicFormRepoInterface
{
    public interface IFormRepo
    {
        Task<FormsTable> GetFormByIdAsync(int formId);
        Task<List<SectionTable>> GetSectionsByFormIdAsync(int formId);
        Task<List<QuestionBank>> GetQuestionsBySectionIdAsync(int sectionId);
        Task<List<AnswerOption>> GetAnswerOptionsByQuestionIdAsync(int questionId);
        Task<FormsTable> GetSourceTemplateById(int formId);

        UserCredential GetUserByEmail(string email);

        bool ValidateUserCredentials(string email, string password);

    }
}
