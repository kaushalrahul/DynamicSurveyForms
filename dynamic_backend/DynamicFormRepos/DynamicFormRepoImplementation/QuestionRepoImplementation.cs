using DynamicFormPresentation.Models;
using DynamicFormRepos.DynamicFormRepoInterface;
using Microsoft.EntityFrameworkCore;

namespace DynamicFormRepos.DynamicFormRepoImplementation
{
    public class QuestionRepoImplementation : IQuestionRepoInterface
    {
        private readonly SDirectContext _context;

        public QuestionRepoImplementation(SDirectContext context)
        {
            _context = context;
        }



        public async Task<QuestionBank> AddQuestionAsync(QuestionBank question)
        {
            _context.QuestionBanks.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<AnswerOption> AddAnswerOptionAsync(AnswerOption answerOption)
        {
            _context.AnswerOptions.Add(answerOption);
            await _context.SaveChangesAsync();
            return answerOption;
        }

        public async Task<AnswerMaster> AddAnswerMasterAsync(AnswerMaster answerMaster)
        {
            _context.AnswerMasters.Add(answerMaster);
            await _context.SaveChangesAsync();
            return answerMaster;
        }

        public async Task<QuestionBank> GetQuestionByIdAsync(int id)
        {
            return await _context.Set<QuestionBank>()
                .Include(q => q.AnswerMasters)
                    .ThenInclude(am => am.AnswerOption)
                        .ThenInclude(ao => ao.AnswerType)
                .FirstOrDefaultAsync(q => q.Id == id);
        }



        public async Task UpdateQuestionAsync(QuestionBank question)
        {
            _context.QuestionBanks.Update(question);
            await _context.SaveChangesAsync();
        }


        public async Task<AnswerOption> GetAnswerOptionByValueAsync(string value)
        {
            return await _context.AnswerOptions
                .Include(ao => ao.AnswerType)
                .FirstOrDefaultAsync(ao => ao.OptionValue == value);
        }

        public async Task UpdateAnswerOptionAsync(AnswerOption answerOption)
        {
            _context.AnswerOptions.Update(answerOption);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> DeleteQuestionAsync(int id)
        {
            // Fetch the question by ID, including related entities
            var question = await _context.QuestionBanks
                .Include(q => q.AnswerMasters)
                .ThenInclude(am => am.AnswerOption)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
                return false;

            // Delete related SectionQuestionMappings
            var sectionQuestionMappings = _context.SectionQuestionMappings
                .Where(sqm => sqm.QuestionId == id);

            if (sectionQuestionMappings.Any())
            {
                _context.SectionQuestionMappings.RemoveRange(sectionQuestionMappings);
            }

            // Delete related AnswerMasters and AnswerOptions
            if (question.AnswerMasters != null && question.AnswerMasters.Any())
            {
                foreach (var answerMaster in question.AnswerMasters)
                {
                    if (answerMaster.AnswerOption != null)
                    {
                        _context.AnswerOptions.Remove(answerMaster.AnswerOption);
                    }

                    _context.AnswerMasters.Remove(answerMaster);
                }
            }

            // Delete the question itself
            _context.QuestionBanks.Remove(question);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true;
        }
        //public async Task<bool> DeleteQuestionAsync(int id)
        //{
        //    // Fetch the question by ID, including related entities
        //    var question = await _context.QuestionBanks
        //        .Include(q => q.AnswerMasters)
        //        .ThenInclude(am => am.AnswerOption)
        //        .FirstOrDefaultAsync(q => q.Id == id);

        //    if (question == null)
        //        return false;

        //    // Delete related AnswerMasters and AnswerOptions
        //    if (question.AnswerMasters != null && question.AnswerMasters.Any())
        //    {
        //        foreach (var answerMaster in question.AnswerMasters)
        //        {
        //            if (answerMaster.AnswerOption != null)
        //            {
        //                _context.AnswerOptions.Remove(answerMaster.AnswerOption);
        //            }

        //            _context.AnswerMasters.Remove(answerMaster);
        //        }
        //    }

        //    // Delete the question itself
        //    _context.QuestionBanks.Remove(question);

        //    // Save changes to the database
        //    await _context.SaveChangesAsync();

        //    return true;
        //}

        public async Task<bool> AddQuestionToSectionAsync(int sectionId, int questionId)
        {
            // Check if the mapping already exists
            var existingMapping = await _context.SectionQuestionMappings
                .FirstOrDefaultAsync(sqm => sqm.SectionId == sectionId && sqm.QuestionId == questionId);

            if (existingMapping != null)
                return false; // Mapping already exists

            var mapping = new SectionQuestionMapping
            {
                SectionId = sectionId,
                QuestionId = questionId
            };

            _context.SectionQuestionMappings.Add(mapping);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<QuestionBank>> GetAllQuestionAsync(int userId)
        {
            return await _context.QuestionBanks
                .Where(q => q.userId == userId)
                .Include(q => q.AnswerMasters)
                    .ThenInclude(am => am.AnswerOption)
                        .ThenInclude(ao => ao.AnswerType)
                .ToListAsync();
        }




    }
}
