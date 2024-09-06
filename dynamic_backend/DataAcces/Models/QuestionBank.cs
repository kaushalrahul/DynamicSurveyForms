namespace DynamicFormPresentation.Models;

public partial class QuestionBank
{
    public int Id { get; set; }

    public bool? Active { get; set; }

    public int? Slno { get; set; }

    public string? Questions { get; set; }

    public bool? Required { get; set; }

    public string? DataType { get; set; }

    public string? Constraints { get; set; }

    public string? ConstraintValue { get; set; }

    public string? WarningMessage { get; set; }

    public string? TextBoxSize { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int userId { get; set; }

    public virtual ICollection<AnswerMaster> AnswerMasters { get; set; } = new List<AnswerMaster>();

    public virtual ICollection<SectionQuestionMapping> SectionQuestionMappings { get; set; } = new List<SectionQuestionMapping>();
}
