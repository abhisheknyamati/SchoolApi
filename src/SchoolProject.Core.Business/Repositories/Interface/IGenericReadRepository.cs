
using System.Linq.Expressions;

namespace SchoolProject.Core.Business.Repositories.Interface
{
    public interface IGenericReadRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int id);
        public Task<T?> IsDuplicateEmailAsync(Expression<Func<T, bool>> emailPredicate);
    }
}