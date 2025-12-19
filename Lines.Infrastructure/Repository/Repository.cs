using Lines.Application.Interfaces;
using Lines.Domain.Models.Common;
using Lines.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Lines.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseModel, new()
{
    private readonly ApplicationDBContext _context;
    private readonly DbSet<T> _dbSet;
    private readonly IUserStateService _userStateService;

    public Repository(ApplicationDBContext context, IUserStateService userStateService)
    {
        _userStateService = userStateService;
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public IQueryable<T> Get()
    {
        return _dbSet.Where(entity => !entity.IsDeleted);
    }

    public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
    {
        return Get().Where(predicate);
    }

    public IQueryable<TResult> SelectWhere<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
    {
        return Get()
            .Where(predicate)
            .Select(selector);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedBy = _userStateService.UserId;
        entity.CreatedDate = DateTime.UtcNow;
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public T Add(T entity)
    {
        entity.CreatedBy = _userStateService.UserId;
        entity.CreatedDate = DateTime.UtcNow;
        _dbSet.Add(entity);
        //_context.SaveChanges();
        return entity;
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedBy = _userStateService.UserId;
        entity.UpdatedDate = DateTime.UtcNow;
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entity in entities)
        {
            entity.UpdatedBy = _userStateService.UserId;
            entity.UpdatedDate = now;              // ensure the same time
        }
        _dbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default, bool isHardDelete = false)
    {
        var entity = await GetByIdAsync(id, cancellationToken);

        if (entity == null)
            return;

        if (isHardDelete)
        {
            // Remove from DbSet so EF issues a DELETE statement
            _dbSet.Remove(entity);
            //await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            // Soft delete
            entity.delete(); // Assuming this sets IsDeleted = true
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedBy = _userStateService.UserId;
            await UpdateAsync(entity, cancellationToken);
        }
    }

    public async Task DeleteRangeAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default, bool isHardDelete = false)
    {
        var entities = await _dbSet
            .Where(e => ids.Contains(e.Id) && !e.IsDeleted)
            .ToListAsync(cancellationToken);

        if (!entities.Any())
            return;

        if (isHardDelete)
        {
            _dbSet.RemoveRange(entities);
            //await _context.SaveChangesAsync(cancellationToken);
        }

        else
        {
            foreach (var entity in entities)
            {
                entity.delete();
                entity.UpdatedBy = _userStateService.UserId;
                entity.UpdatedDate = DateTime.UtcNow;
                await UpdateAsync(entity, cancellationToken);
            }
        }

    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return Get().AnyAsync(predicate, cancellationToken);
    }
}
