
using SchoolProject.StudentModule.Business.Repositories.Interfaces;
using SchoolProject.StudentModule.Business.Services.Interfaces;

namespace SchoolProject.StudentModule.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepo _repo;
        public StudentService(IStudentRepo repo)
        {
            _repo = repo;
        }
        public int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        public bool IsDuplicateEmail(string email)
        {
            return _repo.IsDuplicateEmail(email);
        }
    }
}

