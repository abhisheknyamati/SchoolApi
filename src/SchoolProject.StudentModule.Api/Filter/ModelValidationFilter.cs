using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolProject.StudentModule.Api.DTOs;


namespace SchoolProject.StudentModule.Api.Filter
{
    public class ModelValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(err => err.ErrorMessage).ToList()
                );

                context.Result = new BadRequestObjectResult(
                new ValidationError
                {
                    Message = "One or more validation errors occurred.",
                    StatusCode = StatusCodes.Status400BadRequest,
                    ExceptionMessage = "Validation failed.",
                    Errors = errors
                });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { }
    }
}