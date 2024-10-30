namespace SchoolApi.API.DTOs
{
    public class ErrorDetails
    {
        public ErrorDetails()
        {
            Message = string.Empty;
            ExceptionMessage = string.Empty;
        }

        public Guid TraceId { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public PathString Instance { get; set; }
        public string ExceptionMessage { get; set; }
    }
}