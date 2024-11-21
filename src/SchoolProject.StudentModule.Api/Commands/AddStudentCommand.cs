using System.Text.Json.Serialization;
using MediatR;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Models.ENUM;

namespace SchoolProject.StudentModule.Api.Commands
{
    public class AddStudentCommand : IRequest<Student>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; } = true;

        public AddStudentCommand(string firstName, string lastName, string email, string phone, string address, Gender gender, DateTime birthDate, bool isActive, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
            Gender = gender;
            BirthDate = birthDate;
            Age = age;
            IsActive = isActive;
        }
    }
}