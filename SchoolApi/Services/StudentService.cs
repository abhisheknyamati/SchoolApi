using Microsoft.AspNetCore.Mvc;
using SchoolApi.Models;
using SchoolApi.Models.DTOs;
using SchoolApi.Repositories;

namespace SchoolApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepo _repo;
        public StudentService(IStudentRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            var student = await _repo.GetAllStudents();
            return student ?? new List<Student>();
        }

        public async Task<IActionResult> AddStudent(Student student)
        {
            return await _repo.AddStudent(student);
        }

        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            return await _repo.DeleteStudent(studentId);
        }

        public async Task<IActionResult> UpdateDetails(int id, Student student)
        {
            return await _repo.UpdateDetails(id, student);
        }

        public async Task<ActionResult<PagedResponse<Student>>> GetStudents([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm)
        {
            return await _repo.GetStudents(pageNumber, pageSize, searchTerm);
        }

        public async Task<int> CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        public async Task<ActionResult<Student>> GetStudentById(int id){
            return await _repo.GetStudentById(id);
        }
    }
}
