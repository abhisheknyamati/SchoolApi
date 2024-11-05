namespace SchoolApi.API.DTOs
{
    public class ErrorDetails
    {
        public ErrorDetails()
        {
            Message = string.Empty;
            ExceptionMessage = string.Empty;
            Errors = new Dictionary<string, string[]>();
        }

        public Guid TraceId { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public PathString Instance { get; set; }
        public string ExceptionMessage { get; set; }
        
        // Add this property to hold validation errors
        public IDictionary<string, string[]> Errors { get; set; }
    }
}
