namespace SchoolProject.Core.Business.Models
{
    public class ValidationError
    {
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public string ExceptionMessage { get; set; } = string.Empty;
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
    }
}