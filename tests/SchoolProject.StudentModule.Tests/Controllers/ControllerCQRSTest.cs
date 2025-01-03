
using AutoMapper;
using Bogus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolProject.StudentModule.Api.Commands;
using SchoolProject.StudentModule.Api.Controllers;
using SchoolProject.StudentModule.Api.DTOs;
using SchoolProject.StudentModule.Api.Mappers;
using SchoolProject.StudentModule.Business.Models;
using SchoolProject.StudentModule.Business.Services.Interfaces;
using SchoolProject.StudentModule.Tests.Helper;

namespace SchoolProject.StudentModule.Tests.Controllers
{
    [TestClass]
    public class ControllerCQRSTest
    {
        private IMapper _mapper;
        private Faker<Student> _faker;
        private Mock<IMediator> _mediator;
        private Mock<IStudentService> _studentService;
        private StudentControllerCqrs _controller;

        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new StudentProfile());
            });

            _mapper = config.CreateMapper();

            _mediator = new Mock<IMediator>();
            _studentService = new Mock<IStudentService>();
            _controller = new StudentControllerCqrs(_mediator.Object, _studentService.Object, _mapper);

            _faker = StudentFaker.CreateFakeStudent();
        }

        [TestMethod]
        public async Task AddStudentAsync_ShouldReturnOk()
        {
            // Arrange
            var student = _faker.Generate();
            var addStudentDto = _mapper.Map<AddStudentDto>(student);

            _mediator.Setup(m => m.Send(It.IsAny<AddStudentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(student);

            // Act
            var result = await _controller.AddStudent(addStudentDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        
    }
}