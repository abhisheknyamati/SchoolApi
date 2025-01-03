

namespace SchoolProject.Core.Business.Repositories.Interface
{
    public interface IGenericWriteRepository<T> where T : class
    {
        public Task<T> AddAsync(T entity);
        public Task<T> DeleteAsync(T entity);
        public Task<T> UpdateAsync(T entity, T updatedEntity);
        public Task<T?> SoftDeleteAsync(T entity);
    }
}