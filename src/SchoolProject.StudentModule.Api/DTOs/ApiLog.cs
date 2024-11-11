namespace SchoolProject.StudentModule.Api.DTOs
{
    public class ApiLog
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
    }
}