using Auth.Domain.Models.Common;

namespace Auth.Application.Interfaces;

public interface IRepository<T> where T : EntityBase
{
    IQueryable<T> Query();
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

