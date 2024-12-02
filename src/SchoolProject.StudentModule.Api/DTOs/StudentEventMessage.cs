namespace SchoolProject.StudentModule.Api.DTOs
{
    public class StudentEventMessage
    {
        public string EventType { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
    }
}