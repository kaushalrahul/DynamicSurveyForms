namespace DynamicFormServices.Dto
{
    public class AnswerOptionDto
    {
        public string OptionValue { get; set; }
        public int? NextQuestionId { get; set; } 
    }

    public class QuestionDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public int SerialNumber { get; set; } 
        public string ResponseType { get; set; }
        public List<AnswerOptionDto> AnswerOptions { get; set; }
        public string DataType { get; set; }
        public string Constraint { get; set; } 
        public string ConstraintValue { get; set; } 
        public string WarningMessage { get; set; }
        public bool? Required { get; set; }

        public int UserId { get; set; }

    }
}
