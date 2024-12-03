using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.StudentModule.Business.Commands;
using SchoolProject.StudentModule.Api.DTOs;
using SchoolProject.StudentModule.Business.Queries;
using SchoolProject.Core.Business.Models;
using SchoolProject.Core.Business.Commands;

namespace SchoolProject.StudentModule.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentControllerCqrs : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper _mapper;
        public StudentControllerCqrs(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Student>> GetStudentListAsync()
        {
            var students = await mediator.Send(new GetStudentListQuery());
            return students;
        }

        [HttpGet("{id}")]
        public async Task<GetStudentDto> GetStudentByIdAsync(int id)
        {
            var student = await mediator.Send(new GetStudentByIdQuery(){ Id = id });
            var studentDto = _mapper.Map<GetStudentDto>(student);
            return studentDto;
        }

        [HttpPost]
        public async Task<GetStudentDto> AddStudentAsync(AddStudentDto addStudentDto)
        {
            var student = _mapper.Map<Student>(addStudentDto);
            var newStudent = await mediator.Send(new AddStudentCommand(student));
            var studentDto = _mapper.Map<GetStudentDto>(newStudent);
            return studentDto;
        }

        [HttpDelete]
        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await mediator.Send(new DeleteStudentCommand(){ Id = id} );
        }

        [HttpPut]
        public async Task<GetStudentDto> UpdateStudentAsync(int id, UpdateStudentDto updateStudentDto)
        {
            var student = _mapper.Map<Student>(updateStudentDto);
            var updatedStudent = await mediator.Send(new UpdateStudentCommand(id, student));
            var studentDto = _mapper.Map<GetStudentDto>(updatedStudent);
            return studentDto;
        }
    }
}