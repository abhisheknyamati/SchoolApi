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
        public async Task AddStudent_WhenStudentIsValid_AddsStudentSuccessfully()
        {
            // Arrange
            var student = new Student
            {
                FirstName = "Abhishek",
                LastName = "Nyamati",
                Address = "Somewhere",
                Email = "abhishek@nyamati.com",
                Phone = "1234567890",
                IsActive = true,
                BirthDate = DateTime.Now.AddYears(-20),
                Age = 20,
                Gender = Business.Models.ENUM.Gender.MALE
            };

            // Act
            var result = await _repo.AddStudent(student);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Abhishek", result.FirstName);

            var students = await _repo.GetAllStudents();
            Assert.Single(students);
        }

        [Fact]
        public async Task AddStudent_WhenRequiredFieldsAreMissing_ThrowsDbUpdateException()
        {
            // Arrange
            var student = new Student
            {
                LastName = "Doe",
                Email = "johndoe@example.com",
                IsActive = true
            }; 

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(async () => await _repo.AddStudent(student));
        }

        [Fact]
        public async Task UpdateDetails_WhenStudentExists_UpdatesDetailsSuccessfully()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "Abhishek", LastName = "Nyamati", Address = "Airoli", Email = "abhishek@gmail.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 24, IsActive = true };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Act
            student.LastName = "UpdatedName";
            var updatedStudent = await _repo.UpdateDetails(student);

            // Assert
            Assert.NotNull(updatedStudent);
            Assert.Equal("UpdatedName", updatedStudent.LastName);
        }

        [Fact]
        public async Task UpdateDetails_WhenStudentDoesNotExist_ThrowsDbUpdateConcurrencyException()
        {
            // Arrange
            var student = new Student
            {
                Id = 999, 
                FirstName = "Abhishek",
                LastName = "Nyamati",
                Address = "Vashi",
                Email = "abhishek@gmail.com",
                Gender = Business.Models.ENUM.Gender.MALE,
                Phone = "1234567890",
                BirthDate = new DateTime(2000, 1, 1),
                Age = 24,
                IsActive = true
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _repo.UpdateDetails(student));

            // Additional check: verify no entries were added
            Assert.Empty(await _context.Students.ToListAsync());
        }


        [Fact]
        public async Task DeleteStudent_WhenStudentIsActive_SetsIsActiveToFalse()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "Abhishek", LastName = "Nyamati", Address = "Airoli", Email = "abhishek@gmail.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 24, IsActive = true };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.DeleteStudent(student);

            // Assert
            Assert.True(result);
            Assert.False(student.IsActive);
        }

        [Fact]
        public async Task DeleteStudent_WhenStudentIsInactive_ReturnsFalse()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "Abhishek", LastName = "Nyamati", Address = "Airoli", Email = "abhishek@gmail.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 24, IsActive = false };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.DeleteStudent(student);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetStudentById_WhenStudentExists_ReturnsStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "Abhishek", LastName = "Nyamati", Address = "Airoli", Email = "abhishek@gmail.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 24, IsActive = true };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetStudentById(student.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Abhishek", result.FirstName);
        }

        [Fact]
        public async Task GetStudentById_WhenStudentDoesNotExist_ReturnsNull()
        {
            // Act
            var result = await _repo.GetStudentById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetStudents_WhenFilteredAndPaginated_ReturnsFilteredAndPagedResults()
        {
            // Arrange
            _context.Students.AddRange(new List<Student>
            {
                new Student { FirstName = "Abhishek", LastName = "Nyamati", Address = "Vashi", Email = "abhishek@gmail.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 24, IsActive = true },
                new Student { FirstName = "Bob", LastName = "Jones", Address = "Vashi",  Email = "bob@example.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 24, IsActive = true },
                new Student { FirstName = "Charlie", LastName = "Brown", Address = "Bandra", Email = "charlie@example.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 24, IsActive = true },
            });
            await _context.SaveChangesAsync();

            // Act
            var pagedResult = await _repo.GetStudents(1, 2, "Abhi");

            // Assert
            Assert.Single(pagedResult.Data);
            Assert.Equal("Abhishek", pagedResult.Data.First().FirstName);
        }

        [Fact]
        public async Task GetStudents_WhenNoMatches_ReturnsEmptyList()
        {
            // Arrange
            _context.Students.AddRange(new List<Student>
            {
                new Student { FirstName = "Abhishek", LastName = "Nyamati", Address = "Vashi", Email = "abhishek@gmail.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 24, IsActive = true },
                new Student { FirstName = "Bob", LastName = "Jones", Address = "Vashi",  Email = "bob@example.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 24, IsActive = true },
                new Student { FirstName = "Charlie", LastName = "Brown", Address = "Bandra", Email = "charlie@example.com", Gender = Business.Models.ENUM.Gender.MALE, Phone = "1234567890", BirthDate = new DateTime(2000, 1, 1), Age = 24, IsActive = true },
            });
            await _context.SaveChangesAsync();

            // Act
            var pagedResult = await _repo.GetStudents(1, 2, "NonExistentName");

            // Assert
            Assert.Empty(pagedResult.Data);
        }

        [Fact]
        public async Task GetStudents_WhenNegativePageNumberOrSize_ReturnsEmptyPagedResponse()
        {
            // Arrange
            var pageNumber = -1;
            var pageSize = -5;
            var searchTerm = "Abhishek"; 

            // Act
            var result = await _repo.GetStudents(pageNumber, pageSize, searchTerm);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Data); 
            Assert.Equal(0, result.TotalRecords);
        }

    }
}
