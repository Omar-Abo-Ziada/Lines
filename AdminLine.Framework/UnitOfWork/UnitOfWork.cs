using AdminLine.Framework.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Linq.Expressions;
using System.Data;
using Lines.Infrastructure.Context;

namespace AdminLine.Framework.UoW
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IUnitOfWork"/> and <see cref="IUnitOfWork{TContext}"/> interface.
    /// </summary>
    /// <typeparam name="TContext">The type of the db context.</typeparam>
    public class UnitOfWork : IRepositoryFactory, IUnitOfWork
    {
        private bool disposed = false;
        private Dictionary<Type, object> repositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UnitOfWork(ApplicationDBContext context)
        {
            DbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        /// <returns>The instance of type <typeparamref name="TContext"/>.</returns>
        public ApplicationDBContext DbContext { get; }

        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="hasCustomRepository"><c>True</c> if providing custom repositry</param>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            // what's the best way to support custom reposity?
            if (hasCustomRepository)
            {
                var customRepo = DbContext.GetService<IRepository<TEntity>>();
                if (customRepo != null)
                {
                    return customRepo;
                }
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(DbContext);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
        /// <returns>The number of state entries written to the database.</returns>
        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves all changes made in this unit of work to the database.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Saves all changes made in this context to the database with distributed transaction.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
        /// <param name="unitOfWorks">An optional <see cref="IUnitOfWork"/> array.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        public async Task<int> SaveChangesAsync(params IUnitOfWork[] unitOfWorks)
        {
            using (var ts = new TransactionScope())
            {
                var count = 0;
                foreach (var unitOfWork in unitOfWorks)
                {
                    count += await unitOfWork.SaveChangesAsync();
                }

                count += await SaveChangesAsync();

                ts.Complete();

                return count;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // clear repositories
                    if (repositories != null)
                    {
                        repositories.Clear();
                    }

                    // dispose the db context.
                    DbContext.Dispose();
                }
            }

            disposed = true;
        }

        public void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback)
        {
            DbContext.ChangeTracker.TrackGraph(rootEntity, callback);
        }

        public void ClearTracking(object obj)
        {
            DbContext.Entry(obj).State = EntityState.Detached;
        }

        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return await DbContext.Set<TEntity>().AnyAsync(predicate);
        }
        public async Task<List<T>> ExecuteSqlQueryAsync<T>(string sql, params object[] parameters) where T : class, new()
        {
            using var command = DbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            foreach (var param in parameters)
            {
                command.Parameters.Add(param);
            }

            await DbContext.Database.OpenConnectionAsync();

            var result = new List<T>();
            using var reader = await command.ExecuteReaderAsync();
            var props = typeof(T).GetProperties();

            // Build dictionary of available columns in the reader
            var columnNames = Enumerable.Range(0, reader.FieldCount)
                                        .Select(reader.GetName)
                                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

            while (await reader.ReadAsync())
            {
                var obj = new T();
                foreach (var prop in props)
                {
                    if (columnNames.Contains(prop.Name) && !reader.IsDBNull(reader.GetOrdinal(prop.Name)))
                    {
                        var value = reader[prop.Name];

                        // Handle type conversion
                        var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                        try
                        {
                            var safeValue = Convert.ChangeType(value, targetType);
                            prop.SetValue(obj, safeValue);
                        }
                        catch
                        {
                            // if conversion fails (e.g. string->DateTime), try parsing manually
                            if (targetType == typeof(DateTime) && value is string s && DateTime.TryParse(s, out var dt))
                            {
                                prop.SetValue(obj, dt);
                            }
                            else
                            {
                                // fallback: ignore invalid conversion
                                prop.SetValue(obj, null);
                            }
                        }
                    }
                }
                result.Add(obj);
            }

            return result;
        }



        public async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return await DbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }
    }
}
