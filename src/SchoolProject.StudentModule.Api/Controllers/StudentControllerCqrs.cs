using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Core.Business.Constants;
using SchoolProject.Core.Business.ExceptionHandler;
using SchoolProject.StudentModule.Api.Commands;
using SchoolProject.StudentModule.Api.DTOs;
using SchoolProject.StudentModule.Api.Queries;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Pagination;
using SchoolProject.StudentModule.Business.Services.Interfaces;

namespace SchoolProject.StudentModule.Api.Controllers
{
    [Route("api/CQRS")]
    [ApiController]
    public class StudentControllerCqrs : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IStudentService service;
        private readonly IMapper mapper;
        public StudentControllerCqrs(IMediator mediator, IStudentService service, IMapper mapper)
        {
            this.mediator = mediator;
            this.service = service;
            this.mapper = mapper;
        }
        /// <summary>
        /// Get all students
        /// </summary>
        /// <response code="200">Returns all students</response>
        /// <response code="401">Unauthorized Access</response>
        /// <returns>All students</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetStudentDto>>> GetStudentList()
        {
            var students = await mediator.Send(new GetStudentListQuery());
            var getStudentDto = mapper.Map<IEnumerable<GetStudentDto>>(students);
            foreach (var student in getStudentDto)
            {
                student.Age = service.CalculateAge(student.BirthDate);
            }
            return Ok(getStudentDto);
        }

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <param name="id">Enter valid student id</param>
        /// <response code="200">Returns student</response>
        /// <response code="404">Student Not found</response>
        /// <returns>Student</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetStudentDto>> GetStudentById(int id)
        {
            var student = await mediator.Send(new GetStudentByIdQuery() { Id = id });
            if (student == null)
            {
                return NotFound(ErrorMsgConstant.StudentNotFound);
            }
            var studentDto = mapper.Map<GetStudentDto>(student);
            studentDto.Age = service.CalculateAge(studentDto.BirthDate);
            return Ok(studentDto);
        }

        /// <summary>
        /// Add new student
        /// </summary>
        /// <param name="student">All fields are required</param>
        /// <response code="200">Returns added student</response>
        /// <response code="400">Validation Error</response>
        /// <response code="401">Unauthorized Access</response>
        /// <response code="409">Conflict Error: If email already exists</response>
        /// <returns>Added student</returns>
        /// <exception cref="EmailAlreadyRegistered"></exception>
        [HttpPost]
        public async Task<ActionResult<GetStudentDto>> AddStudent(AddStudentDto student)
        {
            var studentModule = mapper.Map<Student>(student);
            if (service.IsDuplicateEmail(studentModule.Email))
            {
                throw new EmailAlreadyRegistered(ErrorMsgConstant.EmailAlreadyExists);
            }
            var newStudent = await mediator.Send(new AddStudentCommand(studentModule));
            var getStudentDto = mapper.Map<GetStudentDto>(newStudent);
            getStudentDto.Age = service.CalculateAge(getStudentDto.BirthDate);
            return Ok(getStudentDto);
        }
        /// <summary>
        /// Delete student by id
        /// </summary>
        /// <param name="id">Enter student id to delete</param>
        /// <response code="200">Returns deleted student</response>
        /// <response code="400">Validation Error</response>
        /// <response code="401">Unauthorized Access</response>
        /// <response code="404">If student not found</response>
        /// <response code="409">If student already deleted</response>
        /// <returns>Deleted student</returns>
        /// <exception cref="StudentAlreadyDeleted"></exception>
        [HttpDelete]
        public async Task<ActionResult<GetStudentDto>> DeleteStudent(int id)
        {
            var existingStudent = await mediator.Send(new GetStudentByIdQuery() { Id = id });
            if (existingStudent != null)
            {
                var deletedStudent = await mediator.Send(new DeleteStudentCommand(existingStudent));
                if (deletedStudent != null)
                {
                    var getStudentDto = mapper.Map<GetStudentDto>(deletedStudent);
                    return getStudentDto;
                }
                throw new StudentAlreadyDeleted(ErrorMsgConstant.StudentAlreadyDeleted);
            }
            return NotFound(ErrorMsgConstant.StudentNotFound);
        }
        
        /// <summary>
        /// Update student by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="student"></param>
        /// <response code="200">Returns updated student</response>
        /// <response code="400">Validation Error</response>
        /// <response code="401">Unauthorized Access</response>
        /// <response code="404">If student not found</response>
        /// <response code="409">If email already exists</response>
        /// <returns>Updated student</returns>
        /// <exception cref="EmailAlreadyRegistered"></exception>
        [HttpPut]
        public async Task<ActionResult<GetStudentDto>> UpdateStudent(int id, UpdateStudentDto student)
        {
            var existingStudent = await mediator.Send(new GetStudentByIdQuery() { Id = id });
            if (existingStudent != null)
            {
                var studentModule = mapper.Map<Student>(student);
                if (service.IsDuplicateEmail(studentModule.Email))
                {
                    throw new EmailAlreadyRegistered(ErrorMsgConstant.EmailAlreadyExists);
                }
                var updatedStudent = await mediator.Send(new UpdateStudentCommand(id, studentModule));
                var getStudentDto = mapper.Map<GetStudentDto>(updatedStudent);
                getStudentDto.Age = service.CalculateAge(getStudentDto.BirthDate);
                return getStudentDto;
            }
            return NotFound(ErrorMsgConstant.StudentNotFound);
        }

        /// <summary>
        /// Get students with pagination
        /// </summary>
        /// <param name="query"></param>
        /// <response code="200">Returns students with pagination</response>
        /// <response code="401">Unauthorized Access</response>
        /// <returns>Students with pagination</returns>
        [HttpGet("pagination")]
        public async Task<ActionResult<PagedResponse<GetStudentDto>>> GetStudentsPagination([FromQuery] GetStudentsPaginationQuery query)
        {
            if (query.PageNumber < 1)
            {
                return BadRequest(ErrorMsgConstant.PaginationPageNumer);
            }

            if (query.PageSize <= 0 || query.PageSize > 100)
            {
                return BadRequest(ErrorMsgConstant.PaginationPageSize);
            }
            var students = await mediator.Send(query);
            var getStudentDto = mapper.Map<List<GetStudentDto>>(students.Data);
            foreach (var student in getStudentDto)
            {
                student.Age = service.CalculateAge(student.BirthDate);
            }
            PagedResponse<GetStudentDto> pagedResponse = new PagedResponse<GetStudentDto>(getStudentDto, students.PageNumber, students.PageSize, students.TotalRecords);
            return Ok(pagedResponse);
        }
    }
}