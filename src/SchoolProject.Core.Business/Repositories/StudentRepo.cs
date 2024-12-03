
using Microsoft.EntityFrameworkCore;
using SchoolProject.Core.Business.Data;
using SchoolProject.Core.Business.Models;
using SchoolProject.Core.Business.Pagination;
using SchoolProject.Core.Business.Repositories.Interfaces;
namespace SchoolProject.Core.Business.Repositories
{
    public class StudentRepo : IStudentRepo
    {
        private readonly StudentModuleDbContext _context;
        public StudentRepo(StudentModuleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> AddStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteStudent(Student student)
        {
            if (student.IsActive)
            {
                student.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Student> UpdateDetails(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<PagedResponse<Student>> GetStudents(int pageNumber, int pageSize, string searchTerm)
        {
            var query = _context.Students.AsQueryable().Where(i => i.IsActive);

            if (searchTerm != null)
            {
                query = query.Where(i => i.FirstName.Contains(searchTerm) || i.LastName.Contains(searchTerm) || i.Age.ToString() == searchTerm || i.Email.Contains(searchTerm));
            }

            var totalRecords = await query.CountAsync();

            var students = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedResponse = new PagedResponse<Student>(students, pageNumber, pageSize, totalRecords);

            return pagedResponse;
        }

        public async Task<Student?> GetStudentById(int id)
        {
            Student? student = await _context.Students.FirstOrDefaultAsync(u => u.Id == id);
            return student;
        }

        public bool IsDuplicateEmail(string email)
        {
            return _context.Students.Any(i => i.Email == email);
        }
    }
}
