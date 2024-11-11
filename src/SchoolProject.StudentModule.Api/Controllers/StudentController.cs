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
using System.Net.Mime;

namespace SchoolApi.API.Controllers
{
    // [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Adds a new student to the system.
        /// </summary>
        /// <remarks>Creates a new student record based on the provided information.</remarks>
        /// <param name="studentDto">The details of the student to add.</param>
        /// <response code="200">Returns the newly created student</response>
        /// <response code="500">Student not created due to server error</response>
        /// <returns>The details of the newly created student.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetStudentDto), 200)]
        public async Task<IActionResult> AddStudent([FromBody] AddStudentDto studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            student.Age = _service.CalculateAge(studentDto.BirthDate.Value);

            if(_repo.IsDuplicateEmail(student.Email))
            {
                throw new EmailAlreadyRegistered(ErrorMsgConstant.EmailAlreadyExists);
            }
            var addedStudent = await _repo.AddStudent(student);
            if (addedStudent == null)
            {
                throw new Exception(ErrorMsgConstant.StudentNotCreated);
            }
            var response = _mapper.Map<GetStudentDto>(addedStudent);
            return Ok(response);
        }

        /// <summary>
        /// Deletes a student by ID.
        /// </summary>
        /// <param name="studentId">The ID of the student to delete.</param>
        /// <response code="200">Returns the details of the deleted student</response>
        /// <response code="404">Student not found</response>
        /// <response code="500">Student already deleted or other error</response>
        /// <returns>The details of the deleted student.</returns>
        [HttpDelete("{studentId}")]
        [ProducesResponseType(typeof(Student), 200)]
        public async Task<ActionResult> DeleteStudent(int studentId)
        {
            var requiredStudent = await _repo.GetStudentById(studentId);
            if (requiredStudent == null)
            {
                return NotFound(ErrorMsgConstant.StudentNotFound);
            }
            var success = await _repo.DeleteStudent(requiredStudent);
            if (!success)
            {
                throw new StudentAlreadyDeleted(ErrorMsgConstant.StudentAlreadyDeleted);
            }
            var response = _mapper.Map<GetStudentDto>(requiredStudent);
            return Ok(response);
        }

        /// <summary>
        /// Updates the details of a student.
        /// </summary>
        /// <param name="id">The ID of the student to update.</param>
        /// <param name="studentDto">The updated details of the student.</param>
        /// <response code="200">Returns the updated student details</response>
        /// <response code="404">Student not found</response>
        /// <response code="422">Validation error</response>
        /// <response code="500">Student not updated due to server error</response>
        /// <returns>The updated details of the student.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Student), 200)]
        public async Task<IActionResult> UpdateDetails(int id, [FromBody] UpdateStudentDto studentDto)
        {
            var existingStudent = await _repo.GetStudentById(id);
            if (existingStudent == new Student())
            {
                return NotFound(ErrorMsgConstant.StudentNotFound);
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

            if(_repo.IsDuplicateEmail(studentDto.Email))
            {
                throw new EmailAlreadyRegistered(ErrorMsgConstant.EmailAlreadyExists);
            }

            var success = await _repo.UpdateDetails(existingStudent);
            var response = _mapper.Map<GetStudentDto>(success);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a paginated list of students.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of students per page.</param>
        /// <param name="searchTerm">Optional search term for filtering students by name.</param>
        /// <response code="200">Returns the list of students</response>
        /// <response code="404">Student list is empty</response>
        /// <response code="500">Error in pagination</response>
        /// <returns>A paginated list of students.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Student>), 200)]
        public async Task<ActionResult<PagedResponse<Student>>> GetPagedStudents([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string searchTerm = "")
        {
            if (pageNumber < 1)
            {
                throw new PageNumberException(ErrorMsgConstant.PaginationPageNumer);
            }

            if (pageSize <= 0 || pageSize > 100)
            {
                throw new PageSizeException(ErrorMsgConstant.PaginationPageSize);
            }

            PagedResponse<Student> result = await _repo.GetStudents(pageNumber, pageSize, searchTerm);

            // if (result.Data.Count == 0)
            // {
            //     return NotFound(ErrorMsgConstant.StudentListEmpty);
            // }

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a student by ID.
        /// </summary>
        /// <param name="id">The ID of the student to retrieve.</param>
        /// <response code="200">Returns the student details</response>
        /// <response code="404">Student not found</response>
        /// <returns>The details of the student with the specified ID.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Student), 200)]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _repo.GetStudentById(id);
            if (student == null)
            {
                return NotFound(ErrorMsgConstant.StudentNotFound);
            }
            var response = _mapper.Map<GetStudentDto>(student);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves all students.
        /// </summary>
        /// <response code="200">Returns the list of students</response>
        /// <response code="404">Student list is empty</response>
        /// <response code="500">Error in pagination</response>
        /// <returns>A list of all students.</returns>
        [HttpGet("getStudents")]
        [ProducesResponseType(typeof(IEnumerable<Student>), 200)]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _repo.GetAllStudents();
            if (students == null || !students.Any())
            {
                return NotFound(ErrorMsgConstant.StudentListEmpty);
            }
            var response = _mapper.Map<IEnumerable<GetStudentDto>>(students);
            return Ok(response);
        }
    }
}
