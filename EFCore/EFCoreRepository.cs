using EFCore.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EFCore
{
    public class EFCoreRepository<TEntity> : IEFCoreRepository<TEntity> where TEntity : class, IEntity, new()
    {
        public IEFCoreDbContext EFCoreDbContext { get { return _dbContextProvider.GetDbContext(); } }

        public DbSet<TEntity> DbSet => EFCoreDbContext.Set<TEntity>();

        public IQueryable<TEntity> AsQueryable() => DbSet.AsQueryable();

        private readonly IEFCoreDbContextProvider<IEFCoreDbContext> _dbContextProvider;

        public EFCoreRepository(IEFCoreDbContextProvider<IEFCoreDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = DbSet.Where(predicate).FirstOrDefault();
            return entity;
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var entity = await DbSet.Where(predicate).FirstOrDefaultAsync(cancellationToken);
            return entity;
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = DbSet.Where(predicate).ToList();
            return entities;
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var entities = await DbSet.Where(predicate).ToListAsync(cancellationToken);
            return entities;
        }

        public TEntity Add(TEntity entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public List<TEntity> Add(List<TEntity> entities)
        {
            DbSet.AddRange(entities);
            return entities;
        }

        public async Task<List<TEntity>> AddAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(entities, cancellationToken);
            return entities;
        }

        public TEntity Update(TEntity entity)
        {
            DbSet.Update(entity);
            return entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbSet.Update(entity);
            return Task.FromResult(entity);
        }

        public List<TEntity> Update(List<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            return entities;
        }

        public Task<List<TEntity>> UpdateAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            DbSet.UpdateRange(entities);
            return Task.FromResult(entities);
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public void Remove(List<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public Task RemoveAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            DbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }
    }


    public class EFCoreRepository<TEntity, TKey> : EFCoreRepository<TEntity>, IEFCoreRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        public EFCoreRepository(IEFCoreDbContextProvider<IEFCoreDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public TEntity? Get(TKey id)
        {
            var entity = DbSet.Find(id);
            return entity;
        }

        public async Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await DbSet.FindAsync([id], cancellationToken: cancellationToken);
            return entity;
        }

        public bool Remove(TKey id)
        {
            var entity = DbSet.Find(id);
            if (entity == null)
            {
                return false;
            }
            DbSet.Remove(entity);
            return true;
        }

        public async Task<bool> RemoveAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await DbSet.FindAsync([id], cancellationToken: cancellationToken);
            if (entity == null)
            {
                return false;
            }
            DbSet.Remove(entity);
            return true;
        }

        public bool Remove(List<TKey> ids)
        {
            var entities = DbSet.Where(e => ids.Contains(e.Id)).ToList();
            if (entities.Count == 0)
            {
                return false;
            }
            DbSet.RemoveRange(entities);
            return true;
        }

        public async Task<bool> RemoveAsync(List<TKey> ids, CancellationToken cancellationToken = default)
        {
            var entities = await DbSet.Where(e => ids.Contains(e.Id)).ToListAsync(cancellationToken: cancellationToken);
            if (entities.Count == 0)
            {
                return false;
            }
            DbSet.RemoveRange(entities);
            return true;
        }
    }
}
