using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.Models;
using SchoolApi.Models.DTOs;
using SchoolApi.Models.ENUM;
using SchoolApi.Models.Validators;
using SchoolApi.Services;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;
        private readonly IMapper _mapper;
        public StudentController(IStudentService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }


        [HttpGet("getStudents")]
        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            var students = await _service.GetAllStudents();
            return students;
        }


        [HttpPost("addStudent")]
        public async Task<IActionResult> AddStudent([FromBody] AddStudentDto studentDto)
        {
            StudentValidator validator = new StudentValidator();

            ValidationResult result = validator.Validate(studentDto);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    return BadRequest("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                }
            }

            Student student = _mapper.Map<Student>(studentDto);

            student.Age = await _service.CalculateAge(studentDto.BirthDate);

            return await _service.AddStudent(student);
        }

        [HttpDelete("softDelete")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            return await _service.DeleteStudent(studentId);
        }

        [HttpPut("updateDetails")]
        public async Task<IActionResult> UpdateDetails(int id, AddStudentDto studentDto)
        {
            StudentValidator validator = new StudentValidator();
            ValidationResult result = validator.Validate(studentDto);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    return BadRequest("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                }
            }

            Student student = _mapper.Map<Student>(studentDto);

            int age = await _service.CalculateAge(studentDto.BirthDate);
            student.Age = age;

            return await _service.UpdateDetails(id, student);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<Student>>> GetStudents([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1, [FromQuery] string searchTerm = "")
        {
            return await _service.GetStudents(pageNumber, pageSize, searchTerm);
        }

        [HttpGet("getStudentById")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            return await _service.GetStudentById(id);
        }
    }
}
