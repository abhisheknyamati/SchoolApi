using System.Security.Claims;
using SchoolProject.UserModule.Business.Models;

namespace SchoolProject.UserModule.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(string username, string password);
    }
}