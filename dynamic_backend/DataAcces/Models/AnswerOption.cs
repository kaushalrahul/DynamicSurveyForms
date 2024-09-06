namespace DynamicFormPresentation.Models;

public partial class AnswerOption
{
    public int Id { get; set; }

    public bool? Active { get; set; }

    public int? AnswerTypeId { get; set; }

    public string? OptionValue { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<AnswerMaster> AnswerMasters { get; set; } = new List<AnswerMaster>();

    public virtual AnswerType? AnswerType { get; set; }
}
