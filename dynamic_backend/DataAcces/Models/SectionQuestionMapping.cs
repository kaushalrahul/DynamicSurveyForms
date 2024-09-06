namespace DynamicFormPresentation.Models;

public partial class SectionQuestionMapping
{
    public int Id { get; set; }

    public int? QuestionId { get; set; }

    public int? SectionId { get; set; }

    public virtual QuestionBank? Question { get; set; }

    public virtual SectionTable? Section { get; set; }
}
