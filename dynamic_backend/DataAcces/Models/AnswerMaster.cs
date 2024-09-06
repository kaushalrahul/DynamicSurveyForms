using System;
using System.Collections.Generic;

namespace DynamicFormPresentation.Models;

public partial class AnswerMaster
{
    public int Id { get; set; }

    public int? QuestionId { get; set; }

    public int? AnswerOptionId { get; set; }

    public int? NextQuestionId { get; set; }

    public virtual AnswerOption? AnswerOption { get; set; }

    public virtual QuestionBank? Question { get; set; }

    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();
}
