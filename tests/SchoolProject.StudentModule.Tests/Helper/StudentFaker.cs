using Bogus;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Models.ENUM;

namespace SchoolProject.StudentModule.Tests.Helper
{
    public class StudentFaker
    {
        public static Faker<Student> CreateFakeStudent()
        {
            return new Faker<Student>()
                .RuleFor(s => s.Id, f => f.IndexFaker + 1)
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.LastName, f => f.Name.LastName())
                .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.FirstName, s.LastName))
                .RuleFor(s => s.Phone, f => $"{f.PickRandom("7", "8", "9")}{f.Random.Number(100000000, 999999999)}")
                .RuleFor(s => s.Address, f => f.Address.FullAddress())
                .RuleFor(s => s.Gender, f => f.PickRandom<Gender>())
                .RuleFor(s => s.BirthDate, f => f.Date.Past(50, DateTime.Now.AddYears(-18)))
                .RuleFor(s => s.IsActive, f => true);
        }
    }
}