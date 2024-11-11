
using FluentValidation;
using SchoolProject.UserModule.Api.DTOs;

namespace SchoolProject.UserModule.Api.Validators
{
    public class UserValidator : AbstractValidator<PostUserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name).Length(1, 15).WithMessage("Please specify a valid name");
            RuleFor(x => x.Password).NotNull().Length(4, 15).WithMessage("Please specify a valid pass name");
            RuleFor(x => x.Email).NotNull().EmailAddress().WithMessage("Please specify a valid email");
            RuleFor(x => x.IsAdmin).NotNull().WithMessage("Please specify a valid admin status (true/false)");
        }
    }

}