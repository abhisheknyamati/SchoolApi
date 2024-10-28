using SchoolApi.Business.Models;
using SchoolApi.Business.Data;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Business.Pagination;

namespace SchoolApi.Business.Repositories
{
    public class StudentRepo : IStudentRepo
    {
        private readonly SchoolAPIDbContext _context;
        public StudentRepo(SchoolAPIDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> AddStudent(Student student)
        {
            if (student == null)
            {
                return null;
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteStudent(int studentId)
        {
            Student student = await _context.Students.FindAsync(studentId);
            if (student != null && student.IsActive)
            {
                student.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateDetails(int id, Student student)
        {
            Student requiredStudent = await _context.Students.FindAsync(id);
            if (requiredStudent != null)
            {
                requiredStudent.FirstName = student.FirstName;
                requiredStudent.LastName = student.LastName;
                requiredStudent.Address = student.Address;
                requiredStudent.BirthDate = student.BirthDate;
                requiredStudent.Phone = student.Phone;
                requiredStudent.Email = student.Email;
                requiredStudent.Gender = student.Gender;
                requiredStudent.Age = student.Age;
                await _context.SaveChangesAsync();
                return true; 
            }
            return false; 
        }

        public async Task<PagedResponse<Student>> GetStudents(int pageNumber, int pageSize, string searchTerm)
        {
            var query = _context.Students.AsQueryable();

            if (searchTerm != null)
            {
                query = query.Where(i => i.FirstName.Contains(searchTerm) || i.LastName.Contains(searchTerm) || i.Age.ToString() == searchTerm || i.Email.Contains(searchTerm));
            }

            var totalRecords = await query.CountAsync();

            var students = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedResponse = new PagedResponse<Student>(students, pageNumber, pageSize, totalRecords);

            return pagedResponse;
        }

        public async Task<Student> GetStudentById(int id)
        {
            return await _context.Students.FindAsync(id);
        }
    }
}
