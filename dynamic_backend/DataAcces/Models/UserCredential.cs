using System;
using System.Collections.Generic;

namespace DynamicFormPresentation.Models;

public partial class UserCredential
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int? DeletedBy { get; set; }

    public virtual ICollection<FormsTable> FormsTables { get; set; } = new List<FormsTable>();
}
