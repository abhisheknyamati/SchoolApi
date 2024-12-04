using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.StudentModule.Api.Commands;
using SchoolProject.StudentModule.Api.DTOs;
using SchoolProject.StudentModule.Api.Queries;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Services.Interfaces;

namespace SchoolProject.StudentModule.Api.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet]
        public async Task<IEnumerable<GetStudentDto>> GetStudentListAsync()
        {
            var students = await mediator.Send(new GetStudentListQuery());
            var getStudentDto = mapper.Map<IEnumerable<GetStudentDto>>(students);
            foreach (var student in getStudentDto)
            {
                student.Age = service.CalculateAge(student.BirthDate);
            }
            return getStudentDto;
        }

        [HttpGet("{id}")]
        public async Task<GetStudentDto> GetStudentByIdAsync(int id)
        {
            var student = await mediator.Send(new GetStudentByIdQuery(){ Id = id });
            var studentDto = mapper.Map<GetStudentDto>(student);
            studentDto.Age = service.CalculateAge(studentDto.BirthDate);
            return studentDto;
        }

        [HttpPost]
        public async Task<GetStudentDto> AddStudentAsync(AddStudentDto student)
        {
            var studentModule = mapper.Map<Student>(student);
            var newStudent = await mediator.Send(new AddStudentCommand(studentModule));
            var getStudentDto = mapper.Map<GetStudentDto>(newStudent);
            return getStudentDto;
        }

        [HttpDelete]
        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await mediator.Send(new DeleteStudentCommand(){ Id = id} );
        }

        [HttpPut]
        public async Task<GetStudentDto> UpdateStudentAsync(int id, UpdateStudentDto student)
        {
            var studentModule = mapper.Map<Student>(student);
            var updatedStudent = await mediator.Send(new UpdateStudentCommand(id, studentModule));
            var getStudentDto = mapper.Map<GetStudentDto>(updatedStudent);
            getStudentDto.Age = service.CalculateAge(getStudentDto.BirthDate);
            return getStudentDto;
        }
    }
}