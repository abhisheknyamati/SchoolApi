using Microsoft.EntityFrameworkCore;
using SchoolApi.Business.Data;
using SchoolApi.Business.Models;
using SchoolApi.Business.Repositories;

namespace SchoolApi.Test
{
    public class StudentRepoTest : IAsyncLifetime
    {
        private SchoolAPIDbContext _context;
        private IStudentRepo _repo;

        public async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<SchoolAPIDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new SchoolAPIDbContext(options);
            _repo = new StudentRepo(_context);
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [Fact]
        public async Task GetAllStudents_WhenCalled_ReturnsAllStudents()
        {
            // Arrange
            _context.Students.AddRange(new List<Student>
            {
                new Student { FirstName = "Abhi", LastName = "Nyamati", Address = "Thane", Email = "abhishek@gmail.com", Phone = "1234567890", IsActive = true, BirthDate = DateTime.Now, Age = 20, Gender = Business.Models.ENUM.Gender.MALE },
                new Student { FirstName = "Abhi1", LastName = "Nyamati1", Address = "Thane", Email = "abhishek1@gmail.com", Phone = "1234567899", IsActive = true, BirthDate = DateTime.Now, Age = 20, Gender = Business.Models.ENUM.Gender.MALE },
            });

            await _context.SaveChangesAsync();

            // Act
            var students = await _repo.GetAllStudents();

            // Assert
            Assert.Equal(2, students.Count());
        }

        [Fact]
        public async Task GetAllStudents_WhenDatabaseIsEmpty_ReturnsEmptyList()
        {
            // Act
            var students = await _repo.GetAllStudents();

            // Assert
            Assert.Empty(students);
        }

        [Fact]
        public async Task AddStudent_WhenCalled_AddsStudentSuccessfully()
        {
            // Arrange
            var student = new Student { FirstName = "Abhi", LastName = "Nyamati", Address = "Thane", Email = "abhishek@gmail.com", Phone = "1234567890", IsActive = true, BirthDate = DateTime.Now, Age = 20, Gender = Business.Models.ENUM.Gender.MALE };

            // Act
            var result = await _repo.AddStudent(student);
            await _context.SaveChangesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Abhi", result.FirstName);
            var students = await _repo.GetAllStudents();
            Assert.Single(students);
        }

        [Fact]
        public async Task AddStudent_WithInvalidStudentData_ReturnsNull()
        {
            // Act
            var result = await _repo.AddStudent(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteStudent_WhenCalled_SoftDeletesStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "Abhi", LastName = "Nyamati", Address = "Thane", Email = "abhishek@gmail.com", Phone = "1234567890", IsActive = true, BirthDate = DateTime.Now, Age = 20, Gender = Business.Models.ENUM.Gender.MALE };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.DeleteStudent(student.Id);

            // Assert
            Assert.True(result);
            var deletedStudent = await _repo.GetStudentById(student.Id);
            Assert.False(deletedStudent.IsActive);
        }

        [Fact]
        public async Task DeleteStudent_WithNonExistingId_ReturnsFalse()
        {
            // Act
            var result = await _repo.DeleteStudent(3);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateDetails_WhenCalled_UpdatesStudentSuccessfully()
        {
            // Arrange
            var student = new Student { FirstName = "Abhi", LastName = "Nyamati", Address = "Thane", Email = "abhishek@gmail.com", Phone = "1234567890", IsActive = true, BirthDate = DateTime.Now, Age = 20, Gender = Business.Models.ENUM.Gender.MALE };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var updatedStudent = new Student { FirstName = "Maxx", LastName = "Power", Address = "New Address", BirthDate = student.BirthDate, Phone = student.Phone, Email = student.Email, Gender = student.Gender, Age = 25 };

            // Act
            var result = await _repo.UpdateDetails(student.Id, updatedStudent);

            // Assert
            Assert.True(result);
            var fetchedStudent = await _repo.GetStudentById(student.Id);
            Assert.Equal("Maxx", fetchedStudent.FirstName);
        }

        [Fact]
        public async Task UpdateDetails_WithNonExistingId_ReturnsFalse()
        {
            // Act
            var result = await _repo.UpdateDetails(-1, new Student { FirstName = "Maxx", LastName = "Power", Address = "New Address" });

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetStudents_WithPagination_ReturnsPagedResponse()
        {
            // Arrange
            for (int i = 1; i <= 10; i++)
            {
                _context.Students.Add(new Student { FirstName = $"Student{i}", LastName = $"Last{i}", Address = $"Address{i}", Age = i, BirthDate = DateTime.Now, Email = $"email{i}@gmail.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = $"123456789{i}", IsActive = true });
            }
            await _context.SaveChangesAsync();

            // Act
            var pagedResponse = await _repo.GetStudents(pageNumber: 1, pageSize: 5, searchTerm: "");

            // Assert
            Assert.Equal(5, pagedResponse.Data.Count);
            Assert.Equal(10, pagedResponse.TotalRecords);
        }

        [Fact]
        public async Task GetStudentById_WithExistingId_ReturnsStudent()
        {
            // Arrange
            var student = new Student { FirstName = "Abhi", LastName = "Nyamati", Address = "Thane", Email = "abhishek@gmail.com", Phone = "1234567890", IsActive = true, BirthDate = DateTime.Now, Age = 20, Gender = Business.Models.ENUM.Gender.MALE };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetStudentById(student.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Abhi", result.FirstName);
        }

        [Fact]
        public async Task GetStudentById_WithNonExistingId_ReturnsNull()
        {
            // Act
            var result = await _repo.GetStudentById(-1);

            // Assert
            Assert.Null(result);
        }
    }
}