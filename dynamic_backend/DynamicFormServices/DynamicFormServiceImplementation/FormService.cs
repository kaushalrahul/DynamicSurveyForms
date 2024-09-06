using DynamicFormRepos.DynamicFormRepoInterface;
using DynamicFormServices.Dto;
using DynamicFormServices.DynamicFormServiceInterface;

namespace DynamicFormServices.DynamicFormServiceImplementation
{
    public class FormService: IFormService
    {
        private readonly IFormRepo _formRepository;

        public FormService(IFormRepo formRepository)
        {
            _formRepository = formRepository;
        }
        
        public async Task<FormDetailsDto> GetFormDetailsByFormIdAsync(int formId)
        {
            var form = await _formRepository.GetFormByIdAsync(formId);
            if (form == null)
            {
                return null;
            }
            var sections = await _formRepository.GetSectionsByFormIdAsync(formId);

            var sectionDetails = new List<SectionDetailDto>();

            foreach (var section in sections)
            {
                var questions = await _formRepository.GetQuestionsBySectionIdAsync(section.Id);
                

                var questionDetails = new List<QuestionDetailDto>();

                foreach (var question in questions)
                {
                    var answerOptions = await _formRepository.GetAnswerOptionsByQuestionIdAsync(question.Id);
                    questionDetails.Add(new QuestionDetailDto
                    {
                        Id = question.Id,
                        QuestionText = question.Questions,
                        AnswerOptions = answerOptions.Select(a => new AnswerOptionsDto
                        {
                            Id = a.Id,
                            OptionValue = a.OptionValue,
                            AnswerType = a.AnswerType.TypeName
                        }).ToList()
                    });
                }

                sectionDetails.Add(new SectionDetailDto
                {
                    Id = section.Id,
                    SectionName = section.SectionName,
                    Questions = questionDetails
                });
            }

            return new FormDetailsDto
            {
                Id = form.Id,
                FormName = form.FormName,
                Description = form.Comments,
                Sections = sectionDetails,
                Version=form.Version,
            };
        }



        public async Task<FetchFormDto> GetFormById_next_question(int formId)
        {


            var formEntity = await _formRepository.GetSourceTemplateById(formId);

            if (formEntity == null)
            {
                return null;
            }

            var formDto = new FetchFormDto
            {
                Id = formEntity.Id,
                Name = formEntity.FormName,
                Description = formEntity.Comments,
                IsPublish = formEntity.IsPublish,
                Version = formEntity.Version,
                Active = formEntity.Active,

                Sections = formEntity.SectionTables
                    .Where(section => section.Active == true) // Filter inactive sections
                    .Select(section => new FormSectionDto
                    {
                        Id = section.Id,
                        SectionName = section.SectionName,
                        Description = section.Description,
                        Slno = section.Slno,
                        Active = section.Active,

                        Questions = section.SectionQuestionMappings
                            // Filter inactive question-section mappings
                            .Select(qsm => new FormQuestionDto
                            {
                                Id = qsm.Question.Id,
                                Question = qsm.Question.Questions,
                                slno = qsm.Question.Slno,
                                //AnswerTypeId = qsm.Question.AnswerTypeId,
                                DataType = qsm.Question.DataType,
                                Constraint = qsm.Question.Constraints,
                                ConstraintValue = qsm.Question.ConstraintValue,
                                WarningMessage = qsm.Question.WarningMessage,
                               
                                Required = qsm.Question.Required,
                                AnswerOptions = qsm.Question.AnswerMasters
                                    .Select(am => new FormAnswerOptionDto
                                    {
                                        Id = am.AnswerOption.Id,
                                        OptionValue = am.AnswerOption.OptionValue,
                                        NextQuestionId = am.NextQuestionId,
                                        AnswerTypeId = am.AnswerOption.AnswerTypeId,

                                    }).ToList()
                            }).ToList()
                    }).ToList()
            };

            return formDto;
        }


    }
}









