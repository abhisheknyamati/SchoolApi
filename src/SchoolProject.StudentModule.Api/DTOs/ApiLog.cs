namespace SchoolProject.StudentModule.Api.DTOs
{
    public class ApiLog
    {
        public string Method { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
    }
}