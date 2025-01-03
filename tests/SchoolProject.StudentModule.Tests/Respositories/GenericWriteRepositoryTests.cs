

using Bogus;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Core.Business.Repositories;
using SchoolProject.StudentModule.Business.Data;
using SchoolProject.StudentModule.Business.Models;

namespace SchoolProject.StudentModule.Tests.Respositories
{
    [TestClass]
    public class GenericWriteRepositoryTests : IAsyncDisposable
    {
        private StudentModuleWriteDbContext _context;
        private GenericWriteRepository<Student> _repository;
        private Faker<Student> _faker;
        
        [TestInitialize]
        public async Task TestInitialize()
        {
            var options = new DbContextOptionsBuilder<StudentModuleWriteDbContext>()
                .UseInMemoryDatabase(databaseName: "WriteTestDatabase")
                .Options;

            _context = new StudentModuleWriteDbContext(options);
            _repository = new GenericWriteRepository<Student>(_context);

            _faker = new Faker<Student>()
                .RuleFor(s => s.Id, f => f.IndexFaker + 1)
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.LastName, f => f.Name.LastName())
                .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.FirstName, s.LastName))
                .RuleFor(s => s.Phone, f => $"{f.PickRandom("7", "8", "9")}{f.Random.Number(100000000, 999999999)}")
                .RuleFor(s => s.Address, f => f.Address.FullAddress())
                .RuleFor(s => s.BirthDate, f => f.Date.Past(50, DateTime.Now.AddYears(-18)))
                .RuleFor(s => s.IsActive, f => true);

            var fakeStudents = _faker.Generate(10);

            _context.Students.AddRange(fakeStudents);
            await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.Database.EnsureDeletedAsync();
                await _context.DisposeAsync();
            }
        }

        [TestMethod]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var newStudent = _faker.Generate();

            // Act
            var result = await _repository.AddAsync(newStudent);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newStudent.FirstName, result.FirstName);
            Assert.AreEqual(newStudent.Email, result.Email);
            Assert.AreEqual(10, result.Phone.Length);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            // Arrange
            var studentToRemove = _faker.Generate();
            await _repository.AddAsync(studentToRemove);

            // Act
            var result = await _repository.DeleteAsync(studentToRemove);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(studentToRemove.Id, result.Id);
            Assert.AreEqual(studentToRemove.FirstName, result.FirstName);
            
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            var existingStudent = _faker.Generate();
            await _repository.AddAsync(existingStudent);

            var updatedStudent = _faker.Generate();
            updatedStudent.Id = existingStudent.Id;
            updatedStudent.FirstName = "Updated";
            
            // Act
            var result = await _repository.UpdateAsync(existingStudent, updatedStudent);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(existingStudent.Id, result.Id);
            Assert.AreEqual(updatedStudent.FirstName, result.FirstName);
        }

        [TestMethod]
        public async Task SoftDeleteAsync_ShouldSoftDeleteEntity()
        {
            // Arrange
            var studentToSoftDelete = _faker.Generate();
            await _repository.AddAsync(studentToSoftDelete);

            // Act
            var result = await _repository.SoftDeleteAsync(studentToSoftDelete);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(studentToSoftDelete.Id, result.Id);
            Assert.IsFalse(result.IsActive);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public async Task SoftDeleteAsync_ShouldThrowIfNoIsActiveProperty()
        {
            // Arrange
            var invalidEntity = new Faker<object>().Generate();

            // Act
            await _repository.SoftDeleteAsync((Student)(object)invalidEntity);
        }

    }
}

