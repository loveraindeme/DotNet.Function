using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EFCore.Contracts
{
    public interface IEFCoreRepository<TEntity> where TEntity : class, IEntity, new()
    {
        DbSet<TEntity> DbSet { get; }

        IQueryable<TEntity> AsQueryable();

        TEntity? Get(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        TEntity Add(TEntity entity);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        List<TEntity> Add(List<TEntity> entities);

        Task<List<TEntity>> AddAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

        TEntity Update(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        List<TEntity> Update(List<TEntity> entities);

        Task<List<TEntity>> UpdateAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

        void Remove(TEntity entity);

        Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

        void Remove(List<TEntity> entities);

        Task RemoveAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
    }

    public interface IEFCoreRepository<TEntity, TKey> : IEFCoreRepository<TEntity> where TEntity : class, IEntity<TKey>, new()
    {
        TEntity? Get(TKey id);

        Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default);

        bool Remove(TKey id);

        Task<bool> RemoveAsync(TKey id, CancellationToken cancellationToken = default);

        bool Remove(List<TKey> ids);

        Task<bool> RemoveAsync(List<TKey> ids, CancellationToken cancellationToken = default);
    }
}
