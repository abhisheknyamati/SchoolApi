using System.Net;
using SchoolApi.API.DTOs;
using FluentValidation;

namespace SchoolApi.API.ExceptionHandler
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        // private Task HandleExceptionAsync(HttpContext context, Exception exception)
        // {
        //     // if (context.Response.HasStarted)
        //     // {
        //     //     return Task.CompletedTask; 
        //     // }

        //     // var traceId = context.TraceIdentifier.ToString(); 
        //     var errorMessage1 = exception.Message;
        //     var traceId = Guid.NewGuid();
        //     _logger.LogError($"TraceId: {traceId}, Path: {context.Request.Path}, Method: {context.Request.Method}, " +
        //                      $"Exception: {exception.Message}, StackTrace: {exception.StackTrace}");

        //     context.Response.ContentType = "application/json";

        //     var (statusCode, errorMessage) = exception switch
        //     {
        //         KeyNotFoundException => (HttpStatusCode.NotFound, "Resource not found."),
        //         UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access."),
        //         _ => (HttpStatusCode.InternalServerError, "Internal Server Error from the custom middleware.")
        //     };

        //     context.Response.StatusCode = (int)statusCode;

        //     var errorDetails = new ErrorDetails
        //     {
        //         TraceId = traceId,
        //         Message = errorMessage,
        //         StatusCode = context.Response.StatusCode,
        //         Instance = context.Request.Path,
        //         ExceptionMessage = errorMessage1
        //     };

        //     return context.Response.WriteAsJsonAsync(errorDetails);
        // }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var traceId = Guid.NewGuid();
            _logger.LogError($"TraceId: {traceId}, Path: {context.Request.Path}, Method: {context.Request.Method}, " +
                             $"Exception: {exception.Message}, StackTrace: {exception.StackTrace}");

            context.Response.ContentType = "application/json";

            var (statusCode, errorMessage, validationErrors) = exception switch
            {
                KeyNotFoundException => (HttpStatusCode.NotFound, "Resource not found.", null),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access.", null),
                ValidationException validationEx => (HttpStatusCode.BadRequest, "One or more validation errors occurred.",
                    validationEx.Errors.ToDictionary(
                        e => e.PropertyName,
                        e => new[] { e.ErrorMessage })),
                _ => (HttpStatusCode.InternalServerError, "Internal Server Error from the custom middleware.", null)
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

            if (validationErrors != null)
            {
                var response = new
                {
                    type = "Abhishek Nyamati",
                    title = errorMessage,
                    status = context.Response.StatusCode,
                    errors = validationErrors,
                    traceId = traceId.ToString()
                };

                return context.Response.WriteAsJsonAsync(response);
            }

            return context.Response.WriteAsJsonAsync(errorDetails);
        }

    }
}
