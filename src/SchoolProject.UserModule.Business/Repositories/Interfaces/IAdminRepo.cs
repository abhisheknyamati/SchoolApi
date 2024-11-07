using SchoolProject.UserModule.Business.Models;

namespace SchoolProject.UserModule.Business.Repositories.Interfaces
{
    public interface IAdminRepo
    {
        Task<IEnumerable<User>> GetAllAdmins();
        Task<bool> DeleteAdmin(User user);
        Task<User> UpdateDetails(User user);
        Task<User> GetAdminById(int id);
        Task<User> AddAdmin(User user);
    }
}