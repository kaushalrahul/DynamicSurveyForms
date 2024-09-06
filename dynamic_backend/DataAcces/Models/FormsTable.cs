using System;
using System.Collections.Generic;

namespace DynamicFormPresentation.Models;

public partial class FormsTable
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public bool? Active { get; set; }

    public string? FormName { get; set; }

    public string? Comments { get; set; }

    public int? Version { get; set; }

    public bool? IsPublish { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();

    public virtual ICollection<SectionTable> SectionTables { get; set; } = new List<SectionTable>();

    public virtual UserCredential? User { get; set; }
}
