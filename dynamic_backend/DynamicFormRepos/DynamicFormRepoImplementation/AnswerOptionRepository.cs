using DynamicFormPresentation.Models;
using DynamicFormRepos.DynamicFormRepoInterface;

namespace DynamicFormRepos.DynamicFormRepoImplementation
{
    public class AnswerOptionRepository : IAnswerOptionRepository
    {
        private readonly SDirectContext _context;

        public AnswerOptionRepository(SDirectContext context)

        {
            _context = context;
        }

        public async Task<AnswerOption?> GetByIdAsync(int id)
        {
            return await _context.Set<AnswerOption>().FindAsync(id);
        }
    }
}
