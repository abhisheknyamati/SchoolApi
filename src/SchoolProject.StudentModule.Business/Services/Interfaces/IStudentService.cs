
namespace SchoolProject.StudentModule.Business.Services.Interfaces
{
    public interface IStudentService
    {
        int CalculateAge(DateTime birthDate);
        bool IsDuplicateEmail(string email);
    }
}
