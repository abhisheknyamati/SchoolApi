
using SchoolProject.UserModule.Business.Models;

namespace SchoolProject.UserModule.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(User user);
        string HashPasswordWithSalt(string password);
        bool VerifyPassword(string storedHash, string password);
    }
}