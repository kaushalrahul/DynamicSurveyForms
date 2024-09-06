namespace DynamicFormServices.Dto
{
    public class SubmitResponseDto
    {
        /* public int FormID { get; set; }
         public string Email { get; set; }
         public IList<SubmittedAnswerDto> Responses { get; set; }*/
        public int FormID { get; set; }
        public string Email { get; set; }
        public string Response { get; set; }
    }

    public class SubmittedAnswerDto
    {
        public int QuestionID { get; set; }
        public int AnswerOptionID { get; set; }
    }
}
