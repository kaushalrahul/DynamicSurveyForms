namespace DynamicFormServices.Dto
{
    public class FormDto
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsPublish { get; set; }
        public int? Version { get; set; }
    }
}
