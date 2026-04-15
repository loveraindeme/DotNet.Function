using SqlSugar;
using SugarSqlCore.Abstraction;
using SugarSqlCore.Abstraction.Entities;
using System.Linq.Expressions;

namespace SugarSqlCore
{
    public class SugarSqlRepository<TEntity> : ISugarSqlRepository<TEntity> where TEntity : class, IEntity, new()
    {
        private const int BatchInsertUpdateCount = 1000;
        private readonly ISugarSqlDbContextProvider<ISugarSqlDbContext> _dbContextProvider;

        public ISqlSugarClient SugarSqlClient => GetDbContext();

        public ISugarQueryable<TEntity> DbQueryable => GetDbContext().Queryable<TEntity>();

        public SugarSqlRepository(ISugarSqlDbContextProvider<ISugarSqlDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public ISqlSugarClient GetDbContext()
        {
            return _dbContextProvider.GetDbContext().SugarSqlClient;
        }

        public SimpleClient<TEntity> GetDbSimpleClient()
        {
            return GetDbContext().GetSimpleClient<TEntity>();
        }

        public ISugarQueryable<TEntity> AsQueryable()
        {
            return GetDbSimpleClient().AsQueryable();
        }

        public InsertNavTaskInit<TEntity, TEntity> AsInsertNav(TEntity entity)
        {
            return GetDbContext().InsertNav(entity);
        }

        public InsertNavTaskInit<TEntity, TEntity> AsInsertNav(List<TEntity> entities)
        {
            return GetDbContext().InsertNav(entities);
        }

        public UpdateNavTaskInit<TEntity, TEntity> AsUpdateNav(TEntity entity)
        {
            return GetDbContext().UpdateNav(entity);
        }

        public UpdateNavTaskInit<TEntity, TEntity> AsUpdateNav(List<TEntity> entities)
        {
            return GetDbContext().UpdateNav(entities);
        }

        public DeleteNavTaskInit<TEntity, TEntity> AsDeleteNav(TEntity entity)
        {
            return GetDbContext().DeleteNav(entity);
        }

        public DeleteNavTaskInit<TEntity, TEntity> AsDeleteNav(List<TEntity> entities)
        {
            return GetDbContext().DeleteNav(entities);
        }

        #region 查询

        public async Task<bool> AnyAsync()
        {
            var result = await DbQueryable.AnyAsync();
            return result;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            var result = await DbQueryable.AnyAsync(expression, cancellationToken);
            return result;
        }

        public async Task<TEntity?> FindIncludeAsync<TChildEntity>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, CancellationToken cancellationToken = default) where TChildEntity : class, new()
        {
            var entity = await DbQueryable.Includes(includeExpression).FirstAsync(expression, cancellationToken);
            return entity;
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            var entity = await DbQueryable.FirstAsync(expression, cancellationToken);
            return entity;
        }

        public async Task<TEntity?> FindAsync(dynamic id, CancellationToken cancellationToken = default)
        {
            var entity = await GetDbSimpleClient().GetByIdAsync(id, cancellationToken);
            return entity;
        }

        public async Task<TEntity> GetIncludeAsync<TChildEntity>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, CancellationToken cancellationToken = default) where TChildEntity : class, new()
        {
            var entity = await DbQueryable.Includes(includeExpression).FirstAsync(expression, cancellationToken);
            return entity;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            var entity = await GetDbSimpleClient().GetFirstAsync(expression, cancellationToken);
            return entity;
        }

        public async Task<TEntity> GetAsync(dynamic id, CancellationToken cancellationToken = default)
        {
            var entity = await GetDbSimpleClient().GetByIdAsync(id, cancellationToken);
            return entity;
        }

        public async Task<List<TEntity>> GetListIncludeAsync<TChildEntity>(Expression<Func<TEntity, List<TChildEntity>>> includeExpression, CancellationToken cancellationToken = default) where TChildEntity : class, new()
        {
            var entities = await DbQueryable.Includes(includeExpression).ToListAsync(cancellationToken);
            return entities;
        }

        public async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
        {
            var entities = await GetDbSimpleClient().GetListAsync(cancellationToken);
            return entities;
        }

        public async Task<List<TEntity>> GetListIncludeAsync<TChildEntity>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, CancellationToken cancellationToken = default) where TChildEntity : class, new()
        {
            var entities = await DbQueryable.Includes(includeExpression).Where(expression).ToListAsync(cancellationToken);
            return entities;
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            var entities = await GetDbSimpleClient().GetListAsync(expression, cancellationToken);
            return entities;
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            var count = await GetDbSimpleClient().CountAsync(expression, cancellationToken);
            return count;
        }

        public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            var count = await DbQueryable.CountAsync(cancellationToken);
            return count;
        }

        public async Task<bool> IsAnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await GetDbSimpleClient().IsAnyAsync(expression, cancellationToken);
        }

        #endregion 查询

        #region 新增

        public async Task<bool> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var result = await GetDbSimpleClient().InsertAsync(entity, cancellationToken);
            return result;
        }

        public async Task<bool> InsertIncludeAsync<TChildEntity>(TEntity entity, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, InsertNavOptions? insertNavOptions = null) where TChildEntity : class, IEntity, new()
        {
            var result = await AsInsertNav(entity).Include(includeExpression, insertNavOptions).ExecuteCommandAsync();
            return result;
        }

        public async Task<int> InsertRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var num = 0;
            if (entities.Count > BatchInsertUpdateCount)
            {
                var index = 0;
                while (index < entities.Count && !cancellationToken.IsCancellationRequested)
                {
                    var takeCount = index + BatchInsertUpdateCount > entities.Count ? entities.Count - index : BatchInsertUpdateCount;
                    var partitionEntities = entities
                        .Skip(index)
                        .Take(takeCount)
                        .ToList();
                    var partitionInsertResult = await GetDbSimpleClient().InsertRangeAsync(partitionEntities, cancellationToken);
                    if (partitionInsertResult)
                    {
                        num += partitionEntities.Count;
                    }
                    index += takeCount;
                }
            }
            else
            {
                await GetDbSimpleClient().InsertRangeAsync(entities, cancellationToken);
                num = entities.Count;
            }
            return num;
        }

        public async Task<int> InsertRangeIncludeAsync<TChildEntity>(List<TEntity> entities, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, InsertNavOptions? insertNavOptions = null) where TChildEntity : class, IEntity, new()
        {
            var num = 0;
            if (entities.Count > BatchInsertUpdateCount)
            {
                var index = 0;
                while (index < entities.Count)
                {
                    var takeCount = index + BatchInsertUpdateCount > entities.Count ? entities.Count - index : BatchInsertUpdateCount;
                    var partitionEntities = entities
                        .Skip(index)
                        .Take(takeCount)
                        .ToList();
                    var partitionInsertResult = await AsInsertNav(partitionEntities).Include(includeExpression, insertNavOptions).ExecuteCommandAsync();
                    if (partitionInsertResult)
                    {
                        num += partitionEntities.Count;
                    }
                    index += takeCount;
                }
            }
            else
            {
                await AsInsertNav(entities).Include(includeExpression, insertNavOptions).ExecuteCommandAsync();
                num = entities.Count;
            }
            return num;
        }

        public async Task<TEntity> InsertReturnAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var newEntity = await GetDbSimpleClient().InsertReturnEntityAsync(entity, cancellationToken);
            return newEntity;
        }

        public async Task<TEntity> InsertIncludeReturnAsync<TChildEntity>(TEntity entity, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, InsertNavOptions? insertNavOptions = null) where TChildEntity : class, IEntity, new()
        {
            var newEntity = await AsInsertNav(entity).Include(includeExpression, insertNavOptions).ExecuteReturnEntityAsync();
            return newEntity;
        }

        public async Task<long> InsertReturnIdAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var id = await GetDbSimpleClient().InsertReturnBigIdentityAsync(entity, cancellationToken);
            return id;
        }

        #endregion 新增

        #region 更新

        public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var result = await GetDbSimpleClient().UpdateAsync(entity, cancellationToken);
            return result;
        }

        public async Task<bool> UpdateIncludeAsync<TChildEntity>(TEntity entity, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, UpdateNavOptions? updateNavOptions = null) where TChildEntity : class, IEntity, new()
        {
            var result = await AsUpdateNav(entity).Include(includeExpression, updateNavOptions).ExecuteCommandAsync();
            return result;
        }

        public async Task<bool> UpdateRangeIncludeAsync<TChildEntity>(List<TEntity> entities, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, UpdateNavOptions? updateNavOptions = null) where TChildEntity : class, IEntity, new()
        {
            bool result;
            var num = 0;
            if (entities.Count > BatchInsertUpdateCount)
            {
                var index = 0;
                while (index < entities.Count)
                {
                    var takeCount = index + BatchInsertUpdateCount > entities.Count ? entities.Count - index : BatchInsertUpdateCount;
                    var partitionEntities = entities
                        .Skip(index)
                        .Take(takeCount)
                        .ToList();
                    var partitionInsertResult = await AsUpdateNav(partitionEntities).Include(includeExpression, updateNavOptions).ExecuteCommandAsync();
                    if (partitionInsertResult)
                    {
                        num += partitionEntities.Count;
                    }
                    index += takeCount;
                }
                result = num == entities.Count;
            }
            else
            {
                result = await AsUpdateNav(entities).Include(includeExpression, updateNavOptions).ExecuteCommandAsync();
                return result;
            }
            return result;
        }

        public async Task<TEntity> UpdateReturnAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await GetDbSimpleClient().UpdateAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<TEntity> UpdateIncludeReturnAsync<TChildEntity>(TEntity entity, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, UpdateNavOptions? updateNavOptions = null) where TChildEntity : class, IEntity, new()
        {
            await AsUpdateNav(entity).Include(includeExpression, updateNavOptions).ExecuteCommandAsync();
            return entity;
        }

        public async Task<bool> UpdateRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            bool result;
            var num = 0;
            if (entities.Count > BatchInsertUpdateCount)
            {
                var index = 0;
                while (index < entities.Count)
                {
                    var takeCount = index + BatchInsertUpdateCount > entities.Count ? entities.Count - index : BatchInsertUpdateCount;
                    var partitionEntities = entities
                        .Skip(index)
                        .Take(takeCount)
                        .ToList();
                    var partitionInsertResult = await GetDbSimpleClient().UpdateRangeAsync(partitionEntities, cancellationToken);
                    if (partitionInsertResult)
                    {
                        num += partitionEntities.Count;
                    }
                    index += takeCount;
                }
                result = num == entities.Count;
            }
            else
            {
                result = await GetDbSimpleClient().UpdateRangeAsync(entities, cancellationToken);
                return result;
            }
            return result;
        }

        #endregion 更新

        #region 删除

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression, bool isSoftDelete = true, CancellationToken cancellationToken = default)
        {
            if (isSoftDelete && typeof(IDeletionAuditedEntity).IsAssignableFrom(typeof(TEntity)))
            {
                var result = await GetDbContext().Updateable<TEntity>()
                    .Where(expression)
                    .SetColumns("DeletionTime", DateTime.Now)
                    .SetColumns("DeleterId", CurrentUserAmbient.UserId)
                    .SetColumns("IsDeleted", true)
                    .ExecuteCommandAsync(cancellationToken);
                return result > 0;
            }
            else
            {
                var result = await GetDbSimpleClient().DeleteAsync(expression, cancellationToken);
                return result;
            }
        }

        #endregion
    }

    public class SugarSqlRepository<TEntity, TKey> : SugarSqlRepository<TEntity>, ISugarSqlRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        public SugarSqlRepository(ISugarSqlDbContextProvider<ISugarSqlDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        #region 查询

        public async Task<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id!, cancellationToken);
            return entity;
        }

        public async Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await GetAsync(id!, cancellationToken);
            return entity;
        }

        #endregion 查询



        #region 删除

        public async Task<bool> DeleteAsync(TEntity entity, bool isSoftDelete = true, CancellationToken cancellationToken = default)
        {
            if (isSoftDelete && typeof(IDeletionAuditedEntity).IsAssignableFrom(typeof(TEntity)))
            {
                var idColumn = GetDbContext().EntityMaintenance.GetDbColumnName<TEntity>(nameof(IEntity<TKey>.Id));
                var result = await GetDbContext().Updateable<TEntity>()
                    .Where($"{idColumn} = @rowId", new { rowId = entity.Id })
                    .SetColumns("DeletionTime", DateTime.Now)
                    .SetColumns("DeleterId", CurrentUserAmbient.UserId)
                    .SetColumns("IsDeleted", true)
                    .ExecuteCommandAsync(cancellationToken);
                return result == 1;
            }
            else
            {
                var result = await GetDbSimpleClient().DeleteAsync(entity, cancellationToken);
                return result;
            }
        }

        public async Task<bool> DeleteIncludeAsync<TChildEntity>(TEntity entity, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, DeleteNavOptions? deleteNavOptions = null) where TChildEntity : class, IEntity, new()
        {
            var result = await AsDeleteNav(entity).Include(includeExpression, deleteNavOptions).ExecuteCommandAsync();
            return result;
        }

        public async Task<bool> DeleteAsync(List<TEntity> entities, bool isSoftDelete = true, CancellationToken cancellationToken = default)
        {
            if (isSoftDelete && typeof(IDeletionAuditedEntity).IsAssignableFrom(typeof(TEntity)))
            {
                var result = await GetDbContext().Updateable<TEntity>()
                    .Where(x => entities.Select(entity => entity.Id).Contains(x.Id))
                    .SetColumns("DeletionTime", DateTime.Now)
                    .SetColumns("DeleterId", CurrentUserAmbient.UserId)
                    .SetColumns("IsDeleted", true)
                    .ExecuteCommandAsync(cancellationToken);
                return result == entities.Count;
            }
            else
            {
                var result = await GetDbSimpleClient().DeleteAsync(entities, cancellationToken);
                return result;
            }
        }

        public async Task<bool> DeleteIncludeAsync<TChildEntity>(List<TEntity> entities, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, DeleteNavOptions? deleteNavOptions = null) where TChildEntity : class, IEntity, new()
        {
            var result = await AsDeleteNav(entities).Include(includeExpression, deleteNavOptions).ExecuteCommandAsync();
            return result;
        }

        public async Task<bool> DeleteByIdAsync(TKey id, bool isSoftDelete = true, CancellationToken cancellationToken = default)
        {
            if (isSoftDelete && typeof(IDeletionAuditedEntity).IsAssignableFrom(typeof(TEntity)))
            {
                var idColumn = GetDbContext().EntityMaintenance.GetDbColumnName<TEntity>(nameof(IEntity<TKey>.Id));
                var result = await GetDbContext().Updateable<TEntity>()
                    .Where($"{idColumn} = @rowId", new { rowId = id })
                    .SetColumns("DeletionTime", DateTime.Now)
                    .SetColumns("DeleterId", CurrentUserAmbient.UserId)
                    .SetColumns("IsDeleted", true)
                    .ExecuteCommandAsync(cancellationToken);
                return result == 1;
            }
            else
            {
                var result = await GetDbSimpleClient().DeleteByIdAsync(id, cancellationToken);
                return result;
            }
        }

        public async Task<bool> DeleteByIdAsync(List<TKey> ids, bool isSoftDelete = true, CancellationToken cancellationToken = default)
        {
            if (isSoftDelete && typeof(IDeletionAuditedEntity).IsAssignableFrom(typeof(TEntity)))
            {
                var result = await GetDbContext().Updateable<TEntity>()
                    .Where(x => ids.Contains(x.Id))
                    .SetColumns("DeletionTime", DateTime.Now)
                    .SetColumns("DeleterId", CurrentUserAmbient.UserId)
                    .SetColumns("IsDeleted", true)
                    .ExecuteCommandAsync(cancellationToken);
                return result == ids.Count;
            }
            else
            {
                var result = await GetDbSimpleClient().DeleteAsync(x => ids.Contains(x.Id), cancellationToken);
                return result;
            }
        }

        #endregion 删除
    }
}
