using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Models;
using SchoolApi.Models.DTOs;

namespace SchoolApi.Repositories
{
    public class StudentRepo : IStudentRepo
    {
        private readonly SchoolAPIDbContext _context;
        private readonly IMapper _mapper;
        public StudentRepo(SchoolAPIDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<IActionResult> AddStudent(Student student)
        {
            if (student == null)
            {
                return new BadRequestObjectResult(new { message = "student is null" });
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return new OkObjectResult(new { message = "Added Successfully" });
        }

        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            Student student = await _context.Students.FindAsync(studentId);
            if (student != null)
            {
                student.IsActive = false;
                await _context.SaveChangesAsync();
                return new OkObjectResult(new { message = "student successfully deleted" });

            }
            return new BadRequestObjectResult(new { message = "invalid studnet id" });
        }

        public async Task<IActionResult> UpdateDetails(int id, Student student)
        {
            Student requiredStudnet = await _context.Students.FindAsync(id);
            if (requiredStudnet != null)
            {
                requiredStudnet.FirstName = student.FirstName;
                requiredStudnet.LastName = student.LastName;
                requiredStudnet.Address = student.Address;
                requiredStudnet.BirthDate = student.BirthDate;
                requiredStudnet.Phone = student.Phone;
                requiredStudnet.Email = student.Email;
                requiredStudnet.Gender = student.Gender;
                requiredStudnet.Age = student.Age;
                await _context.SaveChangesAsync();
                return new OkObjectResult(new { message = "saved changes" });
            }
            return new BadRequestObjectResult(new { message = "couldn't save changes" });
        }

        public async Task<ActionResult<PagedResponse<Student>>> GetStudents([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm)
        {
            var query = _context.Students.AsQueryable();

            if (searchTerm != null)
            {
                query = query.Where(i => i.FirstName.Contains(searchTerm) || i.LastName.Contains(searchTerm) || i.Age.ToString() == searchTerm || i.Email.Contains(searchTerm));
            }

            var totalRecords = await query.CountAsync();

            var students = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedResponse = new PagedResponse<Student>(students, pageNumber, pageSize, totalRecords);

            return new OkObjectResult(pagedResponse);
        }

        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return new NotFoundObjectResult(new { message = "Student not found" });
            }
            return new OkObjectResult(student);
        }
    }
}
