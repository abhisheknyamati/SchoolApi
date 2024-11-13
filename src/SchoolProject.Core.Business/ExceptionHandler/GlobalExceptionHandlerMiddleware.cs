
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SchoolProject.Core.Business.Constants;

namespace SchoolProject.Core.Business.ExceptionHandler
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