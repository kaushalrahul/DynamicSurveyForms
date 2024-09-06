using DynamicFormPresentation.Models;
using DynamicFormRepos.DynamicFormRepoInterface;
using Microsoft.EntityFrameworkCore;

namespace DynamicFormRepos.DynamicFormRepoImplementation
{
    public class ResponseFormRepoImplementation :IResponseFormRepoInterface
    {

        private readonly SDirectContext _context;

        public ResponseFormRepoImplementation(SDirectContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<int>> GetResponseIdsByFormIdAsync(int formId)
        {
            return await _context.Responses
                                 .Where(r => r.FormId == formId)
                                 .Select(r => r.Id)
                                 .ToListAsync();
        }

        public async Task<int> GetResponseCountByFormIdAsync(int formId)
        {
            return await _context.Responses
                                 .Where(r => r.FormId == formId)
                                 .CountAsync();
        }

        public async Task<Response> GetResponseByIdAsync(int id)
        {
            return await _context.Set<Response>().FindAsync(id);
        }

        public async Task AddResponseAsync(Response response)
        {
            await _context.Set<Response>().AddAsync(response);
            await _context.SaveChangesAsync();
        }

        /*public async Task<Response> GetResponseByEmailAndFormIdAsync(string email, int formId)
        {
            return await _context.Set<Response>()
                .FirstOrDefaultAsync(r => r.Email == email && r.FormId == formId);
        }

        public async Task AddResponseAsync(Response response)
        {
            await _context.Set<Response>().AddAsync(response);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Response>> GetAllResponsesAsync()
        {
            return await _context.Set<Response>().ToListAsync(); // Implement the method
        }

        public async Task UpdateResponseAsync(Response response)
        {
            _context.Set<Response>().Update(response);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteResponseAsync(int id)
        {
            var response = await GetResponseByIdAsync(id);
            if (response != null)
            {
                _context.Set<Response>().Remove(response);
                await _context.SaveChangesAsync();
            }
        }*/
    }
}
