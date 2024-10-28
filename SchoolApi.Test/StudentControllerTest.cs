using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using SchoolApi.API.Controllers;
using SchoolApi.API.DTOs;
using SchoolApi.Business.Models;
using SchoolApi.Business.Pagination;
using SchoolApi.Business.Services;

namespace SchoolApi.Test
{
    public class StudentControllerTest
    {
        private readonly Mock<IStudentService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly StudentController _controller;

        public StudentControllerTest()
        {
            _mockService = new Mock<IStudentService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new StudentController(_mockService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllStudents_ReturnsOkResult_WithListOfStudents()
        {
            // Arrange
            var students = new List<Student> { new Student { Id = 1, FirstName = "Abhishek" } };
            _mockService.Setup(s => s.GetAllStudents()).ReturnsAsync(students);

            // Act
            var result = await _controller.GetAllStudents();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnStudents = Assert.IsType<List<Student>>(okResult.Value);
            Assert.Single(returnStudents);
        }

        [Fact]
        public async Task AddStudent_ReturnsCreatedResult_WithStudent()
        {
            // Arrange
            var studentDto = new AddStudentDto { FirstName = "Abhishek", LastName = "Nyamati", Address="Airoli", Email="abhishek@gmail.com", Gender=Business.Models.ENUM.Gender.MALE,Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1) };
            var student = new Student { Id = 1, FirstName = "Abhishek", LastName = "Nyamati", Address = "Airoli", Email = "abhishek@gmail.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 24, IsActive = true };
            
            _mockMapper.Setup(m => m.Map<Student>(studentDto)).Returns(student);
            _mockService.Setup(s => s.AddStudent(student)).ReturnsAsync(student);
            _mockService.Setup(s => s.CalculateAge(studentDto.BirthDate)).Returns(24);

            // Act
            var result = await _controller.AddStudent(studentDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnStudent = Assert.IsType<Student>(createdResult.Value);
            Assert.Equal("Abhishek", returnStudent.FirstName);
            Assert.Equal(24, returnStudent.Age);
        }

        [Fact]
        public async Task DeleteStudent_ReturnsOkResult_WhenStudentIsDeleted()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteStudent(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteStudent(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = JObject.FromObject(okResult.Value);
            Assert.Equal("successfully deleted student", resultValue["message"].ToString());
        }

        [Fact]
        public async Task DeleteStudent_ReturnsNotFoundResult_WhenStudentNotFound()
        {
            // Arrange
            int studentId = 999;
            _mockService.Setup(s => s.DeleteStudent(studentId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteStudent(studentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var resultValue = JObject.FromObject(notFoundResult.Value);
            Assert.Equal("Invalid student ID.", resultValue["message"].ToString());
        }


        [Fact]
        public async Task UpdateDetails_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var studentDto = new AddStudentDto { FirstName = "Abhishek1", LastName = "Nyamati1", Address = "Airoli", Email = "abhishek@gmail.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1) };
            var student = new Student { Id = 1, FirstName = "Abhishek", LastName = "Nyamati", Address = "Airoli", Email = "abhishek@gmail.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 22 , IsActive = true};

            _mockMapper.Setup(m => m.Map<Student>(studentDto)).Returns(student);
            _mockService.Setup(s => s.UpdateDetails(It.IsAny<int>(), student)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateDetails(1, studentDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = JObject.FromObject(okResult.Value);
            Assert.Equal("Saved changes.", resultValue["message"].ToString());
        }

        [Fact]
        public async Task GetPagedStudents_ReturnsOkResult_WithPagedResponse()
        {
            // Arrange
            var pagedStudents = new PagedResponse<Student>(
                new List<Student> { new Student { Id = 1, FirstName = "Abhishek", LastName = "Nyamati", Address = "Airoli", Email = "abhishek@gmail.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 22, IsActive = true } },
                pageNumber: 1,
                pageSize: 5,
                totalRecords: 1
            );

            _mockService.Setup(s => s.GetStudents(1, 5, "")).ReturnsAsync(pagedStudents);

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
            Assert.Equal("Abhishek", returnStudents.Data.First().FirstName);
        }

        [Fact]
        public async Task GetStudentById_ReturnsOkResult_WithStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "Abhishek" };
            _mockService.Setup(s => s.GetStudentById(1)).ReturnsAsync(student);

            // Act
            var result = await _controller.GetStudentById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnStudent = Assert.IsType<Student>(okResult.Value);
            Assert.Equal(1, returnStudent.Id);
        }

        [Fact]
        public async Task GetStudentById_ReturnsNotFoundResult_WhenStudentNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetStudentById(It.IsAny<int>())).ReturnsAsync((Student)null);

            // Act
            var result = await _controller.GetStudentById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var resultValue = JObject.FromObject(notFoundResult.Value);
            Assert.Equal("Student not found.", resultValue["message"].ToString());
        }
    }
}
