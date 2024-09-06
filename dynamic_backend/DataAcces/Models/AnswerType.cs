using System;
using System.Collections.Generic;

namespace DynamicFormPresentation.Models;

public partial class AnswerType
{
    public int Id { get; set; }

    public bool? Active { get; set; }

    public string? TypeName { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
}
