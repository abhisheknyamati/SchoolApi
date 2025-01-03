
using Microsoft.AspNetCore.Diagnostics;
using SchoolProject.StudentModule.API.Constants;

namespace SchoolProject.StudentModule.API.ExceptionHandler
{
    public class GlobalExceptionHandler : IExceptionHandler
    { 
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, message) = exception switch
            {
                EmailAlreadyRegistered => (StatusCodes.Status409Conflict, exception.Message),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, exception.Message),
                StudentAlreadyDeleted => (StatusCodes.Status409Conflict, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, ErrorMsgConstant.InternalError)
            };

            var response = new 
            {
                StatusCode = statusCode,
                ErrorMessage = message
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return statusCode != StatusCodes.Status500InternalServerError;
        }
    }
}