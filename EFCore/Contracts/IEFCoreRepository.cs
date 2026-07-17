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

        TEntity Add(TEntity entity, bool autoSave = false);

        Task<TEntity> AddAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        List<TEntity> Add(List<TEntity> entities, bool autoSave = false);

        Task<List<TEntity>> AddAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);

        TEntity Update(TEntity entity, bool autoSave = false);

        Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        List<TEntity> Update(List<TEntity> entities, bool autoSave = false);

        Task<List<TEntity>> UpdateAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);

        void Remove(TEntity entity, bool autoSave = false);

        Task RemoveAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        void Remove(List<TEntity> entities, bool autoSave = false);

        Task RemoveAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
    }

    public interface IEFCoreRepository<TEntity, TKey> : IEFCoreRepository<TEntity> where TEntity : class, IEntity<TKey>, new()
    {
        TEntity? Get(TKey id);

        Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default);

        bool Remove(TKey id, bool autoSave = false);

        Task<bool> RemoveAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default);

        bool Remove(List<TKey> ids, bool autoSave = false);

        Task<bool> RemoveAsync(List<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default);
    }
}
