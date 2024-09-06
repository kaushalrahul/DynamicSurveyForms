using DynamicFormPresentation.Models;
using DynamicFormRepos.DynamicFormRepoInterface;
using Microsoft.EntityFrameworkCore;

namespace DynamicFormRepos.DynamicFormRepoImplementation
{
    public class FormRepo:IFormRepo
    {
        private readonly SDirectContext _context;

        public FormRepo(SDirectContext context)
        {
            _context = context;
        }


        public UserCredential GetUserByEmail(string email)
        {
            return _context.UserCredentials.SingleOrDefault(u => u.Email == email);
        }

        public bool ValidateUserCredentials(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }

            
            return user.Password == password;
        }


        public async Task<FormsTable> GetFormByIdAsync(int formId)
        {
            return await _context.FormsTables.FindAsync(formId);
        }

        public async Task<List<SectionTable>> GetSectionsByFormIdAsync(int formId)
        {
            return await _context.SectionTables
                .Where(s => s.FormId == formId && s.DeletedDate==null)
                .ToListAsync();
        }

        public async Task<List<QuestionBank>> GetQuestionsBySectionIdAsync(int sectionId)
        {
            var questionIds = await _context.SectionQuestionMappings
                .Where(sqm => sqm.SectionId == sectionId)
                .Select(sqm => sqm.QuestionId)
                .ToListAsync();

            return await _context.QuestionBanks
                .Where(q => questionIds.Contains(q.Id))
                .ToListAsync();
        }


        public async Task<List<AnswerOption>> GetAnswerOptionsByQuestionIdAsync(int questionId)
        {
            return await _context.AnswerMasters
                .Where(am => am.QuestionId == questionId)
                .Join(_context.AnswerOptions,
                    am => am.AnswerOptionId,
                    ao => ao.Id,
                   
                    (am, ao) => new
                    {
                        ao.Id,
                        ao.OptionValue,
                       
                        AnswerType = _context.AnswerTypes.FirstOrDefault(at => at.Id == ao.AnswerTypeId) // Ensure AnswerType is not null
                    })
                .Select(x => new AnswerOption
                {
                    Id = x.Id,
                    OptionValue = x.OptionValue,
                    AnswerType = x.AnswerType,
                    
                })
                .ToListAsync();
        }

        public async Task<FormsTable> GetSourceTemplateById(int formId)
        {


            var formEntity = await _context.FormsTables
                                    .Include(f => f.SectionTables
                                        .Where(s => s.Active == true)) // Filter active sections
                                    .ThenInclude(s => s.SectionQuestionMappings)
                                    .ThenInclude(qsm => qsm.Question)
                                        .ThenInclude(q => q.AnswerMasters) // Filter active answer masters
                                    .ThenInclude(am => am.AnswerOption)
                                    .FirstOrDefaultAsync(f => f.Id == formId);




            return formEntity;
        }


    }
}
