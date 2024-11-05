using FluentValidation;
using SchoolApi.API.DTOs;
using SchoolApi.Business.Models.ENUM;

namespace SchoolApi.API.Validators
{
    public class StudentValidator : AbstractValidator<AddStudentDto>
    {
        public StudentValidator()
        {
            RuleFor(x => x.FirstName).Length(1, 15).When(s => string.IsNullOrEmpty(s.FirstName)).WithMessage("Please specify a valid first name");
            RuleFor(x => x.LastName).NotNull().Length(0, 15).WithMessage("Please specify a valid last name");
            RuleFor(x => x.Email).NotNull().EmailAddress().WithMessage("Please specify a valid email");
            RuleFor(x => x.Phone).NotNull().Length(10).WithMessage("Please specify a valid phone number");
            RuleFor(x => x.Address).NotNull().MaximumLength(30).WithMessage("Please specify a valid address");
            RuleFor(x => x.Gender).Must(gender => gender == Gender.MALE || gender == Gender.FEMALE || gender == Gender.OTHER || gender == null)
            .WithMessage("Gender must be Male, Female, or unspecified.");
            RuleFor(x => x.BirthDate).NotNull().WithMessage("Please enter a valid date");
        }
    }

    public class StudentUpdateValidator : AbstractValidator<UpdateStudentDto>
    {
        public StudentUpdateValidator()
        {
            RuleFor(x => x.FirstName).Length(1, 15).When(s => string.IsNullOrEmpty(s.FirstName)).WithMessage("Please specify a valid first name");
            RuleFor(x => x.LastName).Length(1, 15).When(x => x.LastName != null).WithMessage("Please specify a valid last name").When(s => !string.IsNullOrEmpty(s.LastName));
            RuleFor(x => x.Email).EmailAddress().When(x => x.Email != null).WithMessage("Please specify a valid email").When(s => !string.IsNullOrEmpty(s.Email));
            RuleFor(x => x.Phone).Length(10).When(x => x.Phone != null).WithMessage("Please specify a valid phone number").When(s => !string.IsNullOrEmpty(s.Phone));
            RuleFor(x => x.Address).MaximumLength(30).When(x => x.Address != null).WithMessage("Please specify a valid address").When(s => !string.IsNullOrEmpty(s.Address));
            RuleFor(x => x.Gender)
                .Must(gender => gender == Gender.MALE || gender == Gender.FEMALE || gender == Gender.OTHER || gender == null)
                .WithMessage("Gender must be Male, Female, Other, or unspecified.").When(s => !string.IsNullOrEmpty(s.Gender.ToString()));
            RuleFor(x => x.BirthDate).Must(date => date > DateTime.Now).WithMessage("Please enter a valid date").When(s => !string.IsNullOrEmpty(s.BirthDate.ToString()));
        }
    }

}
