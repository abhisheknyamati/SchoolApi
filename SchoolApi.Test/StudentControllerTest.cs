using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolApi.API.Controllers;
using SchoolApi.API.DTOs;
using SchoolApi.Business.Models;
using SchoolApi.Business.Pagination;
using SchoolApi.Business.Repositories;
using SchoolApi.Business.Services;

namespace SchoolApi.Test
{
    public class StudentControllerTest
    {
        private readonly Mock<IStudentRepo> _mockRepo;
        private readonly Mock<IStudentService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly StudentController _controller;

        public StudentControllerTest()
        {
            _mockRepo = new Mock<IStudentRepo>();
            _mockService = new Mock<IStudentService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new StudentController(_mockRepo.Object, _mockMapper.Object, _mockService.Object);
        }

        [Fact]
        public async Task GetAllStudents_ReturnsOkResult_WithListOfStudents()
        {
            // Arrange
            var students = new List<Student> { new Student { Id = 1, FirstName = "Abhishek" } };
            _mockRepo.Setup(repo => repo.GetAllStudents()).ReturnsAsync(students);

            // Act
            var result = await _controller.GetAllStudents();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnStudents = Assert.IsType<List<Student>>(okResult.Value);
            Assert.Single(returnStudents);
        }

        [Fact]
        public async Task AddStudent_ReturnsOkResult_WithStudent()
        {
            // Arrange
            var studentDto = new AddStudentDto { FirstName = "Abhishek", LastName = "Nyamati", Address = "Airoli", Email = "abhishek@gmail.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1) };
            var student = new Student { Id = 1, FirstName = "Abhishek", LastName = "Nyamati", Address = "Airoli", Email = "abhishek@gmail.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 24, IsActive = true };

            _mockMapper.Setup(m => m.Map<Student>(studentDto)).Returns(student);
            _mockRepo.Setup(repo => repo.AddStudent(student)).ReturnsAsync(student);
            _mockService.Setup(s => s.CalculateAge(studentDto.BirthDate.Value)).Returns(24);

            // Act
            var result = await _controller.AddStudent(studentDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnStudent = Assert.IsType<Student>(okResult.Value);
            Assert.Equal("Abhishek", returnStudent.FirstName);
            Assert.Equal(24, returnStudent.Age);
        }

        [Fact]
        public async Task DeleteStudent_ReturnsOkResult_WhenStudentIsDeleted()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "Abhishek" };
            _mockRepo.Setup(repo => repo.GetStudentById(1)).ReturnsAsync(student);
            _mockRepo.Setup(repo => repo.DeleteStudent(student)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteStudent(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(student, okResult.Value);
        }

        [Fact]
        public async Task UpdateDetails_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var studentDto = new UpdateStudentDto { FirstName = "Abhishek1", LastName = "Nyamati1", Address = "Airoli", Email = "abhishek@gmail.com", Phone = "1234567890" };
            var student = new Student { Id = 1, FirstName = "Abhishek", LastName = "Nyamati", Address = "Airoli", Email = "abhishek@gmail.com", Phone = "1234567890", Age = 24 };

            _mockRepo.Setup(repo => repo.GetStudentById(1)).ReturnsAsync(student);
            _mockRepo.Setup(repo => repo.UpdateDetails(student)).ReturnsAsync(student);

            // Act
            var result = await _controller.UpdateDetails(1, studentDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(student, okResult.Value);
        }

        [Fact]
        public async Task GetPagedStudents_ReturnsOkResult_WithPagedResponse()
        {
            // Arrange
            var pagedStudents = new PagedResponse<Student>(
                new List<Student> { new Student { Id = 1, FirstName = "Abhishek" } },
                pageNumber: 1,
                pageSize: 5,
                totalRecords: 1
            );

            _mockRepo.Setup(repo => repo.GetStudents(1, 5, "")).ReturnsAsync(pagedStudents);

            // Act
            var result = await _controller.GetPagedStudents(1, 5, "");

            // Assert
            var actionResult = Assert.IsType<ActionResult<PagedResponse<Student>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnStudents = Assert.IsType<PagedResponse<Student>>(okResult.Value);

            Assert.Single(returnStudents.Data);
            Assert.Equal(1, returnStudents.PageNumber);
            Assert.Equal(5, returnStudents.PageSize);
            Assert.Equal(1, returnStudents.TotalRecords);
        }

        [Fact]
        public async Task GetStudentById_ReturnsOkResult_WithStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "Abhishek" };
            _mockRepo.Setup(repo => repo.GetStudentById(1)).ReturnsAsync(student);

            // Act
            var result = await _controller.GetStudentById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnStudent = Assert.IsType<Student>(okResult.Value);
            Assert.Equal(1, returnStudent.Id);
        }

        [Fact]
        public async Task GetStudentById_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetStudentById(1)).ReturnsAsync((Student)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _controller.GetStudentById(1));
            Assert.Equal("Student not found!!", exception.Message);
        }

    }
}
