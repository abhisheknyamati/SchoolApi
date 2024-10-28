using Microsoft.AspNetCore.Mvc;
using SchoolApi.Business.Models;
using SchoolApi.Business.Services;
using AutoMapper;
using SchoolApi.API.Validators;
using SchoolApi.API.DTOs;
using SchoolApi.Business.Pagination;

namespace SchoolApi.API.Controllers
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
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _service.GetAllStudents();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching students.");
            }
        }

        [HttpPost("addStudent")]
        public async Task<IActionResult> AddStudent([FromBody] AddStudentDto studentDto)
        {
            try
            {
                var validator = new StudentValidator();
                var result = validator.Validate(studentDto);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors.Select(failure =>
                        $"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage}"));
                }

                var student = _mapper.Map<Student>(studentDto);
                student.Age = _service.CalculateAge(studentDto.BirthDate);

                var addedStudent = await _service.AddStudent(student);
                if (addedStudent == null)
                {
                    return BadRequest("Failed to add student.");
                }

                return CreatedAtAction(nameof(GetStudentById), new { id = addedStudent.Id }, addedStudent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the student.");
            }
        }

        [HttpDelete("softDelete")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            try
            {
                var success = await _service.DeleteStudent(studentId);
                if (!success)
                {
                    return NotFound(new { message = "Invalid student ID." });
                }

                return Ok(new { message = "successfully deleted student" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the student.");
            }
        }

        [HttpPut("updateDetails")]
        public async Task<IActionResult> UpdateDetails(int id, [FromBody] AddStudentDto studentDto)
        {
            try
            {
                var validator = new StudentValidator();
                var result = validator.Validate(studentDto);

                if (!result.IsValid)
                {
                    return BadRequest(result.Errors.Select(failure =>
                        $"Property {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage}"));
                }

                var student = _mapper.Map<Student>(studentDto);
                student.Age = _service.CalculateAge(studentDto.BirthDate);

                var success = await _service.UpdateDetails(id, student);
                if (!success)
                {
                    return BadRequest("Couldn't save changes.");
                }

                return Ok(new { message = "Saved changes." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating student details.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<Student>>> GetPagedStudents([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = "")
        {
            try
            {
                if (pageNumber < 1)
                {
                    return BadRequest("Page number must be greater than 0.");
                }

                if (pageSize <= 0 || pageSize > 100)
                {
                    return BadRequest("Page size must be between 1 and 100.");
                }

                PagedResponse<Student> result = await _service.GetStudents(pageNumber, pageSize, searchTerm);

                if (result.Data.Count == 0)
                {
                    return NotFound(new { message = "No students found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching paged students.");
            }
        }

        [HttpGet("getStudentById")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            try
            {
                var student = await _service.GetStudentById(id);
                if (student == null)
                {
                    return NotFound(new { message = "Student not found." });
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching the student.");
            }
        }
    }
}
