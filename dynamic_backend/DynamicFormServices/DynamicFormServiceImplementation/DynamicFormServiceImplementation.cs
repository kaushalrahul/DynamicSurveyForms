using DynamicFormPresentation.Models;
using DynamicFormRepo.DynamicFormRepoInterface;
using DynamicFormService.DynamicFormServiceInterface;
using DynamicFormServices.Dto;

namespace DynamicFormService.DynamicFormServiceImplementation
{
    public class DynamicFormServiceImplementation : IDynamicFormServiceInterface
    {
        private readonly IDynamicFormRepoInterface _dynamicFormRepoInterface;
        public DynamicFormServiceImplementation(IDynamicFormRepoInterface dynamicFormRepoInterface)
        {
            _dynamicFormRepoInterface = dynamicFormRepoInterface;
        }

        public async Task<IEnumerable<UserCredential>> GetAllUsersAsync()
        {
            return await _dynamicFormRepoInterface.GetAllAsync();
        }

        public async Task<IEnumerable<QuestionBank>> GetAllQuestion()
        {
            return await _dynamicFormRepoInterface.GetAllQuestion();
        }

        public async Task<IEnumerable<SectionDto>> GetAllSectionsAsync()
        {
            var sections = await _dynamicFormRepoInterface.GetAllSectionAsync();
            return sections.Select(s => new SectionDto
            {   
                FormId = s.FormId,
                SectionName = s.SectionName,
                Description = s.Description,
                Slno = s.Slno,
                
            });
        }

        public async Task<SectionDto> GetSectionByIdAsync(int id)
        {
            var section = await _dynamicFormRepoInterface.GetBySectionIdAsync(id);
            if (section == null)
            {
                return null;
            }

            return new SectionDto
            {
                
                FormId = section.FormId,
                SectionName = section.SectionName,
                Description = section.Description,
                Slno = section.Slno,
                
            };
        }

        public async Task<FormDto> CreateForm(FormsTable form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            var createdForm = await _dynamicFormRepoInterface.CreateForm(form);
            return new FormDto
            {
                UserId = form.UserId,
                FormName = form.FormName,
                Description = form.Comments,
                Id=form.Id,
                IsPublish = form.IsPublish
            };
            
        }

        public async Task<IEnumerable<FormDto>> GetAllFormsAsync(int userId)
        {
            var forms = await _dynamicFormRepoInterface.GetAllFormsAsync(userId);
            return forms.Select(f => new FormDto
            {
                Id = f.Id,
                UserId = f.UserId,
                FormName = f.FormName,
                Description = f.Comments,
                CreatedOn = f.CreatedOn,
                IsPublish = f.IsPublish
            });
        }


        public async Task<FormDto> GetFormByIdAsync(int id)
        {
            var form = await _dynamicFormRepoInterface.GetFormByIdAsync(id);
            if (form == null)
            {
                return null;
            }

            return new FormDto
            {
                UserId = form.UserId,
                FormName = form.FormName,
                Description = form.Comments,
            };
        }

        public async Task<FormDto> UpdateFormAsync(FormsTable form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            var updatedForm = await _dynamicFormRepoInterface.UpdateFormAsync(form);
            if (updatedForm == null)
            {
                return null;
            }

            return new FormDto
            {
                UserId = updatedForm.UserId,
                FormName = updatedForm.FormName,
                Description = updatedForm.Comments,
                IsPublish = updatedForm.IsPublish
            };
        }

        public async Task<bool> DeleteFormAsync(int id)
        {
            return await _dynamicFormRepoInterface.DeleteFormAsync(id);
        }



        public async Task<int> CreateSectionsAsync(SectionDto sectionDtos)
        {
            if (sectionDtos == null )
            {
                throw new ArgumentNullException(nameof(sectionDtos));
            }

            var section = new SectionTable
            {

                FormId = sectionDtos.FormId,
                SectionName = sectionDtos.SectionName,
                Description = sectionDtos.Description,
                Slno = sectionDtos.Slno,
                CreatedOn = DateTime.UtcNow
            };


               var res=  await _dynamicFormRepoInterface.CreateSectionAsync(section);

            return res.Id;

        }

        public async Task<SectionDto> UpdateSectionAsync(SectionTable section)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            var updatedSection = await _dynamicFormRepoInterface.UpdateSectionAsync(section);
            if (updatedSection == null)
            {
                return null;
            }

            return new SectionDto
            {
               
                FormId = updatedSection.FormId,
                SectionName = updatedSection.SectionName,
                Description = updatedSection.Description,
                Slno = updatedSection.Slno,
                
            };
        }

        public async Task<bool> DeleteSectionAsync(int id)
        {
            return await _dynamicFormRepoInterface.DeleteSectionAsync(id);
        }

    }
}




