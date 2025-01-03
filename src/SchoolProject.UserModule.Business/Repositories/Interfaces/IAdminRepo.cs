using SchoolProject.UserModule.Business.Models;

namespace SchoolProject.UserModule.Business.Repositories.Interfaces
{
    public interface IAdminRepo
    {
        Task<IEnumerable<User?>> GetAllUsers();
        Task<bool> DeleteUser(User user);
        Task<User> UpdateDetails(User user);
        Task<User?> GetUserById(int id);
        Task<User> AddUser(User user);
        Task<User?> GetUserByEmail(string email);
    }
}