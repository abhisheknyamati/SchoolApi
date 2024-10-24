using FluentValidation;
using SchoolApi.Models.DTOs;
using SchoolApi.Models.ENUM;

namespace SchoolApi.Models.Validators
{
    public class StudentValidator : AbstractValidator<AddStudentDto>
    {
        public StudentValidator()
        {
            RuleFor(x => x.FirstName).NotNull().Length(0, 15).WithMessage("Please specify a valid first name");
            RuleFor(x => x.LastName).NotNull().Length(0, 15).WithMessage("Please specify a valid last name");
            RuleFor(x => x.Email).NotNull().EmailAddress().WithMessage("Please specify a valid email");
            RuleFor(x => x.Phone).NotNull().Length(10).WithMessage("Please specify a valid phone number");
            RuleFor(x => x.Address).NotNull().MaximumLength(30).WithMessage("Please specify a valid address");
            RuleFor(x => x.Gender).Must(BeAValidGender).WithMessage("Please give valid gender [MALE/FEMALE/OTHER]");
            RuleFor(x => x.BirthDate).NotNull().WithMessage("Please enter a valid date");
        }

        private bool BeAValidGender(Gender gender)
        {
            if (gender.Equals(Gender.FEMALE)) return true;
            else if (gender.Equals(Gender.MALE)) return true;
            else if (gender.Equals(Gender.OTHER)) return true;
            return false;
        }

        private bool BeAValidBirthDate(DateOnly birthDate)
        {
            return birthDate > new DateOnly();
        }
    }
}
