namespace DynamicFormPresentation.Models;

public partial class SectionTable
{
    public int Id { get; set; }

    public bool? Active { get; set; }

    public int? Slno { get; set; }

    public int? FormId { get; set; }

    public string? SectionName { get; set; }

    public string? Description { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual FormsTable? Form { get; set; }

    public virtual ICollection<SectionQuestionMapping> SectionQuestionMappings { get; set; } = new List<SectionQuestionMapping>();
}
