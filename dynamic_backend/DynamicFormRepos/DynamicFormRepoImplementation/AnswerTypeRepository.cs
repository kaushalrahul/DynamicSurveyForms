using DynamicFormPresentation.Models;
using DynamicFormRepos.DynamicFormRepoInterface;
using Microsoft.EntityFrameworkCore;

namespace DynamicFormRepos.DynamicFormRepoImplementation
{
    public class AnswerTypeRepository:IAnswerTypeRepository
    {


        private readonly SDirectContext _context;

        public AnswerTypeRepository(SDirectContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AnswerType>> GetAllAsync()
        {
            return await _context.AnswerTypes.ToListAsync();
        }

        public async Task<AnswerType?> GetByIdAsync(int id)
        {
            return await _context.AnswerTypes.FindAsync(id);
        }
    }
}
