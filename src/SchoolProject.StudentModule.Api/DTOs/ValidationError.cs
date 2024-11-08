namespace SchoolProject.StudentModule.Api.DTOs
{
    public class ValidationError
    {
   
        public required string Message { get; set; }
        public int StatusCode { get; set; }
        public required string ExceptionMessage { get; set; }
        public IDictionary<string, List<string>>? Errors { get; set; }
   
    }
}