
using SchoolProject.Core.Business.Services.Interfaces;

namespace SchoolProject.Core.Business.Services
{
    public class StudentService : IStudentService
    {
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
    }
}

