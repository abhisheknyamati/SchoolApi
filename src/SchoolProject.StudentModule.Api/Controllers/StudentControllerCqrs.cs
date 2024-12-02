using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using SchoolProject.StudentModule.Api.Commands;
using SchoolProject.StudentModule.Api.DTOs;
using SchoolProject.StudentModule.Api.Queries;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Models.ENUM;
using SchoolProject.StudentModule.Business.Services.Interfaces;

namespace SchoolProject.StudentModule.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentControllerCqrs : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IStudentService service;
        private readonly Plain.RabbitMQ.IPublisher _publisher;
        public StudentControllerCqrs(IMediator mediator, IStudentService service, Plain.RabbitMQ.IPublisher publisher)
        {
            this.mediator = mediator;
            this.service = service;
            _publisher = publisher;
        }

        [HttpGet]
        public async Task<IEnumerable<Student>> GetStudentListAsync()
        {
            var students = await mediator.Send(new GetStudentListQuery());
            return students;
        }

        [HttpGet("{id}")]
        public async Task<Student> GetStudentByIdAsync(int id)
        {
            var student = await mediator.Send(new GetStudentByIdQuery() { Id = id });
            return student;
        }

        [HttpPost]
        public async Task<Student> AddStudentAsync(AddStudentDto student)
        {
            var newStudent = await mediator.Send(new AddStudentCommand(
                firstName: student.FirstName,
                lastName: student.LastName,
                email: student.Email,
                phone: student.Phone,
                address: student.Address,
                birthDate: (DateTime)student.BirthDate,
                age: service.CalculateAge((DateTime)student.BirthDate),
                gender: (Gender)student.Gender,
                isActive: true
            ));

            var studentCreatedMessage = new StudentEventMessage
            {
                EventType = "created",
                StudentId = newStudent.Id,
                StudentName = newStudent.FirstName,
                StudentEmail = newStudent.Email
            };

            var message = JsonConvert.SerializeObject(studentCreatedMessage);
            _publisher.Publish(message, "student.created", null);

            return newStudent;
        }

        [HttpDelete]
        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await mediator.Send(new DeleteStudentCommand() { Id = id });
        }

        [HttpPut]
        public async Task<Student> UpdateStudentAsync(int id, UpdateStudentDto student)
        {
            var updatedStudent = await mediator.Send(new UpdateStudentCommand(id, student));
            return updatedStudent;
        }
    }
}