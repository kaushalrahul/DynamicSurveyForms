public class FormDetailsDto
{
    public int Id { get; set; }
    public string FormName { get; set; }
    public string Description { get; set; }

    public int? Version { get; set; }

    public List<SectionDetailDto> Sections { get; set; }

}

public class SectionDetailDto
{
    public int Id { get; set; }
    public string SectionName { get; set; }
    public List<QuestionDetailDto> Questions { get; set; }
}

public class QuestionDetailDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public List<AnswerOptionsDto> AnswerOptions { get; set; }
}

public class AnswerOptionsDto
{
    public int Id { get; set; }
    public string OptionValue { get; set; }
    public string AnswerType { get; set; }
    public int? NextQuestionId { get; set; }

}
