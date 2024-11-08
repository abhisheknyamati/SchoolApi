
using Microsoft.AspNetCore.Diagnostics;

namespace SchoolProject.StudentModule.API.ExceptionHandler
{
    public class GlobalExceptionHandler : IExceptionHandler
    { 
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if(exception is StudentNotFound)
            {
                var response = new 
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = exception.Message
                };
 
                await httpContext.Response.WriteAsJsonAsync(response);
                return true;
            }
 
            if(exception is EmailAlreadyRegistered)
            {
                var response = new 
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    ErrorMessage = exception.Message
                };
 
                await httpContext.Response.WriteAsJsonAsync(response);
                return true;
            }

            if(exception is UnauthorizedAccessException){
                var response = new 
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    ErrorMessage = exception.Message
                };
 
                await httpContext.Response.WriteAsJsonAsync(response);
                return true;
            }
            return false;
        }
    }
}