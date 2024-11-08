using SchoolProject.UserModule.Business.Models;

namespace SchoolProject.UserModule.Business.Repositories.Interfaces
{
    public interface IAuthRepo
    {
        Task<User> ValidateUser(string email, string password);
    }
}