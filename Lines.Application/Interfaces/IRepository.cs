using System.Linq.Expressions;
using Lines.Domain.Models.Common;

namespace Lines.Application.Interfaces;

public interface IRepository<T> where T : BaseModel, new()
{
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    IQueryable<T> Get(Expression<Func<T, bool>> predicate);
    IQueryable<T> Get();
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    T Add(T entity);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default, bool isHardDelete = false);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    void SaveChanges();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default, bool isHardDelete = false);
    IQueryable<TResult> SelectWhere<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}