using DynamicFormPresentation.Models;

namespace DynamicFormRepos.DynamicFormRepoInterface
{
    public interface IQuestionRepoInterface
    {
        Task<QuestionBank> AddQuestionAsync(QuestionBank question);
        Task<AnswerOption> AddAnswerOptionAsync(AnswerOption answerOption);
        Task<AnswerMaster> AddAnswerMasterAsync(AnswerMaster answerMaster);



        Task<QuestionBank> GetQuestionByIdAsync(int id);


        
        Task UpdateQuestionAsync(QuestionBank question);
        Task<AnswerOption> GetAnswerOptionByValueAsync(string value);
        
        Task UpdateAnswerOptionAsync(AnswerOption answerOption);





        Task<bool> DeleteQuestionAsync(int id);


        Task<bool> AddQuestionToSectionAsync(int sectionId, int questionId);


        Task<List<QuestionBank>> GetAllQuestionAsync(int userId);

    }
}
