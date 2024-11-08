using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using SchoolProject.StudentModule.Business.Repositories.Interfaces;
using SchoolProject.StudentModule.Business.Services.Interfaces;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Pagination;
using SchoolProject.StudentModule.API.ExceptionHandler;
using SchoolProject.StudentModule.Api.DTOs;
using SchoolProject.Api.Filter;
using Microsoft.AspNetCore.Authorization;

namespace SchoolApi.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ModelValidationFilter))]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepo _repo;
        private readonly IStudentService _service;
        private readonly IMapper _mapper;

        public StudentController(IStudentRepo repo, IMapper mapper, IStudentService service)
        {
            _repo = repo;
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("getStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _repo.GetAllStudents();
            if (students == null || !students.Any())
            {
                return NotFound(ErrorMsgConstant.StudentListEmpty);
            }

            return Ok(students);
        }

        [HttpPost("addStudent")]
        public async Task<IActionResult> AddStudent([FromBody] AddStudentDto studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            if (studentDto.BirthDate.HasValue)
            {
                student.Age = _service.CalculateAge(studentDto.BirthDate.Value);
            }

            var addedStudent = await _repo.AddStudent(student);
            if (addedStudent == null)
            {
                throw new Exception(ErrorMsgConstant.StudentNotCreated);
            }

            return Ok(student);
        }

        [HttpDelete("softDelete")]
        public async Task<ActionResult> DeleteStudent(int studentId)
        {
            var requiredStudent = await _repo.GetStudentById(studentId);
            if(requiredStudent == null)
            {
                return NotFound(ErrorMsgConstant.StudentNotFound);
            }
            var success = await _repo.DeleteStudent(requiredStudent);
            if (!success)
            {
                throw new Exception(ErrorMsgConstant.StudentAlreadyDeleted);
            }

            return Ok(requiredStudent);
        }

        [HttpPut("updateDetails")]
        public async Task<IActionResult> UpdateDetails(int id, [FromBody] UpdateStudentDto studentDto)
        {
            var existingStudent = await _repo.GetStudentById(id);
            if (existingStudent == null)
            {
                throw new KeyNotFoundException(ErrorMsgConstant.StudentNotFound);
            }

            if (!string.IsNullOrEmpty(studentDto.FirstName))
                existingStudent.FirstName = studentDto.FirstName;
            if (!string.IsNullOrEmpty(studentDto.LastName))
                existingStudent.LastName = studentDto.LastName;
            if (!string.IsNullOrEmpty(studentDto.Email))
                existingStudent.Email = studentDto.Email;
            if (!string.IsNullOrEmpty(studentDto.Phone))
                existingStudent.Phone = studentDto.Phone;
            if (!string.IsNullOrEmpty(studentDto.Address))
                existingStudent.Address = studentDto.Address;
            if (studentDto.Gender.HasValue)
                existingStudent.Gender = studentDto.Gender.Value;
            if (studentDto.BirthDate.HasValue)
            {
                existingStudent.BirthDate = studentDto.BirthDate.Value;
                existingStudent.Age = _service.CalculateAge(studentDto.BirthDate.Value);
            }

            var success = await _repo.UpdateDetails(existingStudent);
            // if (success == existingStudent)
            // {
            //     return BadRequest(ErrorMsgConstant.StudentNotUpdated);
            // }

            return Ok(success);
        }


        [HttpGet]
        public async Task<ActionResult<PagedResponse<Student>>> GetPagedStudents([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = "")
        {
            if (pageNumber < 1)
            {
                throw new Exception(ErrorMsgConstant.PaginationPageNumer);
            }

            if (pageSize <= 0 || pageSize > 100)
            {
                throw new Exception(ErrorMsgConstant.PaginationPageSize);
            }

            PagedResponse<Student> result = await _repo.GetStudents(pageNumber, pageSize, searchTerm);

            if (result.Data.Count == 0)
            {
                throw new KeyNotFoundException(ErrorMsgConstant.StudentListEmpty);
            }

            return Ok(result);
        }

        [HttpGet("getStudentById")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _repo.GetStudentById(id);
            if (student == null)
            {
                throw new StudentNotFound(ErrorMsgConstant.StudentNotFound);
            }

            return Ok(student);
        }
    }
}
