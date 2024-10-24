using Microsoft.AspNetCore.Mvc;
using SchoolApi.Models;
using SchoolApi.Models.DTOs;

namespace SchoolApi.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudents();
        Task<IActionResult> AddStudent(Student student);
        Task<IActionResult> DeleteStudent(int studentId);
        Task<IActionResult> UpdateDetails(int id, Student studentDto);
        Task<ActionResult<PagedResponse<Student>>> GetStudents([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchTerm);
        Task<int> CalculateAge(DateTime birthDate);
        Task<ActionResult<Student>> GetStudentById(int id);
    }
}
