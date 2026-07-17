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

        public TEntity Add(TEntity entity, bool autoSave = false)
        {
            DbSet.Add(entity);
            if (autoSave)
            {
                EFCoreDbContext.SaveChanges();
            }
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
            if (autoSave)
            {
                await EFCoreDbContext.SaveChangesAsync(cancellationToken);
            }
            return entity;
        }

        public List<TEntity> Add(List<TEntity> entities, bool autoSave = false)
        {
            DbSet.AddRange(entities);
            if (autoSave)
            {
                EFCoreDbContext.SaveChanges();
            }
            return entities;
        }

        public async Task<List<TEntity>> AddAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(entities, cancellationToken);
            if (autoSave)
            {
                await EFCoreDbContext.SaveChangesAsync(cancellationToken);
            }
            return entities;
        }

        public TEntity Update(TEntity entity, bool autoSave = false)
        {
            DbSet.Update(entity);
            if (autoSave)
            {
                EFCoreDbContext.SaveChanges();
            }
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.Update(entity);
            if (autoSave)
            {
                await EFCoreDbContext.SaveChangesAsync(cancellationToken);
            }
            return entity;
        }

        public List<TEntity> Update(List<TEntity> entities, bool autoSave = false)
        {
            DbSet.UpdateRange(entities);
            if (autoSave)
            {
                EFCoreDbContext.SaveChanges();
            }
            return entities;
        }

        public async Task<List<TEntity>> UpdateAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.UpdateRange(entities);
            if (autoSave)
            {
                await EFCoreDbContext.SaveChangesAsync(cancellationToken);
            }
            return entities;
        }

        public void Remove(TEntity entity, bool autoSave = false)
        {
            DbSet.Remove(entity);
            if (autoSave)
            {
                EFCoreDbContext.SaveChanges();
            }
        }

        public async Task RemoveAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);
            if (autoSave)
            {
                await EFCoreDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public void Remove(List<TEntity> entities, bool autoSave = false)
        {
            DbSet.RemoveRange(entities);
            if (autoSave)
            {
                EFCoreDbContext.SaveChanges();
            }
        }

        public async Task RemoveAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.RemoveRange(entities);
            if (autoSave)
            {
                await EFCoreDbContext.SaveChangesAsync(cancellationToken);
            }
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

        public bool Remove(TKey id, bool autoSave = false)
        {
            var entity = DbSet.Find(id);
            if (entity == null)
            {
                return false;
            }
            DbSet.Remove(entity);
            if (autoSave)
            {
                EFCoreDbContext.SaveChanges();
            }
            return true;
        }

        public async Task<bool> RemoveAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entity = await DbSet.FindAsync([id], cancellationToken: cancellationToken);
            if (entity == null)
            {
                return false;
            }
            DbSet.Remove(entity);
            if (autoSave)
            {
                await EFCoreDbContext.SaveChangesAsync(cancellationToken);
            }
            return true;
        }

        public bool Remove(List<TKey> ids, bool autoSave = false)
        {
            var entities = DbSet.Where(e => ids.Contains(e.Id)).ToList();
            if (entities.Count == 0)
            {
                return false;
            }
            DbSet.RemoveRange(entities);
            if (autoSave)
            {
                EFCoreDbContext.SaveChanges();
            }
            return true;
        }

        public async Task<bool> RemoveAsync(List<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = await DbSet.Where(e => ids.Contains(e.Id)).ToListAsync(cancellationToken: cancellationToken);
            if (entities.Count == 0)
            {
                return false;
            }
            DbSet.RemoveRange(entities);
            if (autoSave)
            {
                await EFCoreDbContext.SaveChangesAsync(cancellationToken);
            }
            return true;
        }
    }
}
