
using System.Linq.Expressions;

namespace SchoolProject.Core.Business.Repositories.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
        Task<T?> IsDuplicateEmailAsync(Expression<Func<T, bool>> predicate);
        Task<T?> SoftDeleteAsync(T entity);
    }
}