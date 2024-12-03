using System.Text.Json.Serialization;
using SchoolProject.Core.Business.Models.ENUM;

namespace SchoolProject.StudentModule.Api.DTOs
{
    public class GetStudentDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
    }
}