﻿using System.Text.Json.Serialization;
using SchoolProject.StudentModule.Business.Models.ENUM;


namespace SchoolProject.StudentModule.Business.Models
{
    public class Student
    {
        public int Id { get; set; }
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
    }
}