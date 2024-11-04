using System.Net;
using SchoolApi.API.DTOs;

namespace SchoolApi.API.ExceptionHandler
{
    public class GlobalExceptionHandlerMiddleware 
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // if (context.Response.HasStarted)
            // {
            //     return Task.CompletedTask; 
            // }

            // var traceId = context.TraceIdentifier.ToString(); 
            var traceId = Guid.NewGuid();
            _logger.LogError($"TraceId: {traceId}, Path: {context.Request.Path}, Method: {context.Request.Method}, " +
                             $"Exception: {exception.Message}, StackTrace: {exception.StackTrace}");

            context.Response.ContentType = "application/json";

            var (statusCode, errorMessage) = exception switch
            {
                KeyNotFoundException => (HttpStatusCode.NotFound, "Resource not found."),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access."),
                _ => (HttpStatusCode.InternalServerError, "Internal Server Error from the custom middleware.")
            };

            context.Response.StatusCode = (int)statusCode;

            var errorDetails = new ErrorDetails
            {
                TraceId = traceId,
                Message = errorMessage,
                StatusCode = context.Response.StatusCode,
                Instance = context.Request.Path,
                ExceptionMessage = exception.Message
            };

            return context.Response.WriteAsJsonAsync(errorDetails);
        }
    }
}
