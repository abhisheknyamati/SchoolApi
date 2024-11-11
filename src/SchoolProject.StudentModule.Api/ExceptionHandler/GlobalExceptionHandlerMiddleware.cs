
using Microsoft.AspNetCore.Diagnostics;

namespace SchoolProject.StudentModule.API.ExceptionHandler
{
    public class GlobalExceptionHandler : IExceptionHandler
    { 
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
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

            if(exception is EmailAlreadyRegistered){
                var response = new 
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    ErrorMessage = exception.Message
                };
 
                await httpContext.Response.WriteAsJsonAsync(response);
                return true;
            }

            if(exception is PageSizeException){
                var response = new 
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = exception.Message
                };
 
                await httpContext.Response.WriteAsJsonAsync(response);
                return true;
            }

            if(exception is PageNumberException){
                var response = new 
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = exception.Message
                };
 
                await httpContext.Response.WriteAsJsonAsync(response);
                return true;
            }   

            if(exception is StudentAlreadyDeleted){
                var response = new 
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    ErrorMessage = exception.Message
                };
 
                await httpContext.Response.WriteAsJsonAsync(response);
                return true;
            }
            
            return false;
        }
    }
}