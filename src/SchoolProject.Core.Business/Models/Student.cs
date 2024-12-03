using System.Text.Json.Serialization;
using SchoolProject.Core.Business.Models.ENUM;

namespace SchoolProject.Core.Business.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }  = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
