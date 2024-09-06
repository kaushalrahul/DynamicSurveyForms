using DynamicFormPresentation.Models;
using DynamicFormRepo.DynamicFormRepoInterface;
using Microsoft.EntityFrameworkCore;

namespace DynamicFormRepo.DynamicFormRepoImplementation
{


    public class DynamicFormRepoImplementation : IDynamicFormRepoInterface
    {
        private readonly SDirectContext _context ;

        public DynamicFormRepoImplementation(SDirectContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserCredential>> GetAllAsync()
        {
            return await _context.UserCredentials.ToListAsync();
        }

        public async Task<IEnumerable<QuestionBank>> GetAllQuestion()
        {
            return await _context.QuestionBanks.ToListAsync();
        }

        public async Task<IEnumerable<SectionTable>> GetAllSectionAsync()
        {
            return await _context.SectionTables
                .Where(s => s.DeletedDate == null)
                .ToListAsync();
        }



        public async Task<SectionTable> GetBySectionIdAsync(int id)
        {
            return await _context.SectionTables
                 .Where(s => s.Id == id && s.DeletedDate == null)
                 .FirstOrDefaultAsync();
        }
        public async Task<FormsTable> GetFormByIdAsync(int id)
        {
            return await _context.FormsTables
                .Where(f => f.Id == id && f.DeletedDate == null)
                .FirstOrDefaultAsync();
        }


        public async Task<FormsTable> CreateForm(FormsTable form)
        {
            _context.FormsTables.Add(form);
            await _context.SaveChangesAsync();
            return form;
        }




        public async Task<IEnumerable<FormsTable>> GetAllFormsAsync(int userId)
        {
            return await _context.FormsTables
                .Where(f => f.DeletedDate == null && f.UserId == userId) // Filter by user ID
                .ToListAsync();
        }



        public async Task<FormsTable> UpdateFormAsync(FormsTable form)
        {
            var existingForm = await _context.FormsTables
                .Where(f => f.Id == form.Id && f.DeletedDate == null)
                .FirstOrDefaultAsync();

            if (existingForm == null)
            {
                return null;
            }

            _context.Entry(existingForm).CurrentValues.SetValues(form);
            await _context.SaveChangesAsync();
            return existingForm;
        }

        public async Task<bool> DeleteFormAsync(int id)
        {
            var form = await _context.FormsTables
                .FirstOrDefaultAsync(f => f.Id == id);

            if (form == null)
            {
                return false;
            }

            form.DeletedDate = DateTime.UtcNow;
            _context.FormsTables.Update(form);
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<SectionTable> CreateSectionAsync(SectionTable section)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            _context.SectionTables.Add(section);
            await _context.SaveChangesAsync();

            return section;
            
        }



        public async Task<SectionTable> UpdateSectionAsync(SectionTable section)
        {
            var existingSection = await _context.SectionTables
        .Where(s => s.Id == section.Id && s.DeletedDate == null)
        .FirstOrDefaultAsync();


            if (existingSection == null)
            {
                return null;
            }

            _context.Entry(existingSection).CurrentValues.SetValues(section);
            await _context.SaveChangesAsync();
            return existingSection;
        }

        public async Task<bool> DeleteSectionAsync(int id)
        {
            var section = await _context.SectionTables
                .Include(s => s.SectionQuestionMappings) 
                .FirstOrDefaultAsync(s => s.Id == id);

            if (section == null)
            {
                return false;
            }

             
            section.DeletedDate = DateTime.UtcNow;
            

            _context.SectionTables.Update(section); 

            await _context.SaveChangesAsync();

            return true;
        }

       
    }
}
