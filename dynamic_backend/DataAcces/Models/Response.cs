using System;
using System.Collections.Generic;

namespace DynamicFormPresentation.Models;

public partial class Response
{
    public int Id { get; set; }

    public bool? Active { get; set; }

    public int? FormId { get; set; }

    public string? Email { get; set; }

    public string? Response1 { get; set; }

    public int? AnswerMasterId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual AnswerMaster? AnswerMaster { get; set; }

    public virtual FormsTable? Form { get; set; }
}
