using SqlSugar;
using SugarSqlCore.Abstraction.Entities;
using System.Linq.Expressions;

namespace SugarSqlCore.Abstraction
{
    public interface ISugarSqlRepository<TEntity> where TEntity : class, new()
    {
        ISqlSugarClient SugarSqlClient { get; }

        ISugarQueryable<TEntity> DbQueryable { get; }

        ISqlSugarClient GetDbContext();

        ISugarQueryable<TEntity> AsQueryable();

        InsertNavTaskInit<TEntity, TEntity> AsInsertNav(TEntity entity);

        InsertNavTaskInit<TEntity, TEntity> AsInsertNav(List<TEntity> entities);

        UpdateNavTaskInit<TEntity, TEntity> AsUpdateNav(TEntity entity);

        UpdateNavTaskInit<TEntity, TEntity> AsUpdateNav(List<TEntity> entities);

        DeleteNavTaskInit<TEntity, TEntity> AsDeleteNav(TEntity entity);

        DeleteNavTaskInit<TEntity, TEntity> AsDeleteNav(List<TEntity> entities);

        #region 查询

        Task<bool> AnyAsync();

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        Task<TEntity?> FindIncludeAsync<TChildEntity>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, CancellationToken cancellationToken = default) where TChildEntity : class, new();

        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        Task<TEntity?> FindAsync(dynamic id, CancellationToken cancellationToken = default);

        Task<TEntity> GetIncludeAsync<TChildEntity>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, CancellationToken cancellationToken = default) where TChildEntity : class, new();

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        Task<TEntity> GetAsync(dynamic id, CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetListIncludeAsync<TChildEntity>(Expression<Func<TEntity, List<TChildEntity>>> includeExpression, CancellationToken cancellationToken = default) where TChildEntity : class, new();

        Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetListIncludeAsync<TChildEntity>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, CancellationToken cancellationToken = default) where TChildEntity : class, new();

        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(CancellationToken cancellationToken = default);

        Task<bool> IsAnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        #endregion

        #region 新增

        Task<bool> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<bool> InsertIncludeAsync<TChildEntity>(TEntity entity, Expression<Func<TEntity, List<TChildEntity>>> expression, InsertNavOptions? insertNavOptions = null) where TChildEntity : class, IEntity, new();

        Task<int> InsertRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

        Task<int> InsertRangeIncludeAsync<TChildEntity>(List<TEntity> entities, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, InsertNavOptions? insertNavOptions = null) where TChildEntity : class, IEntity, new();

        Task<TEntity> InsertReturnAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> InsertIncludeReturnAsync<TChildEntity>(TEntity entity, Expression<Func<TEntity, List<TChildEntity>>> expression, InsertNavOptions? insertNavOptions = null) where TChildEntity : class, IEntity, new();

        Task<long> InsertReturnIdAsync(TEntity entity, CancellationToken cancellationToken = default);

        #endregion

        #region 更新

        Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<bool> UpdateIncludeAsync<TChildEntity>(TEntity entity, Expression<Func<TEntity, List<TChildEntity>>> expression, UpdateNavOptions? updateNavOptions = null) where TChildEntity : class, IEntity, new();

        Task<bool> UpdateRangeIncludeAsync<TChildEntity>(List<TEntity> entities, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, UpdateNavOptions? updateNavOptions = null) where TChildEntity : class, IEntity, new();

        Task<TEntity> UpdateReturnAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> UpdateIncludeReturnAsync<TChildEntity>(TEntity entity, Expression<Func<TEntity, List<TChildEntity>>> expression, UpdateNavOptions? updateNavOptions = null) where TChildEntity : class, IEntity, new();
        Task<bool> UpdateRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

        #endregion

        #region 删除

        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression, bool isSoftDelete = true, CancellationToken cancellationToken = default);

        #endregion
    }

    public interface ISugarSqlRepository<TEntity, TKey> : ISugarSqlRepository<TEntity> where TEntity : class, new()
    {
        #region 查询

        Task<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default);

        Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

        #endregion

        #region 新增

        #endregion

        #region 更新

        #endregion

        #region 删除

        Task<bool> DeleteAsync(TEntity entity, bool isSoftDelete = true, CancellationToken cancellationToken = default);

        Task<bool> DeleteIncludeAsync<TChildEntity>(TEntity entity, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, DeleteNavOptions? deleteNavOptions = null) where TChildEntity : class, IEntity, new();

        Task<bool> DeleteAsync(List<TEntity> entities, bool isSoftDelete = true, CancellationToken cancellationToken = default);

        Task<bool> DeleteIncludeAsync<TChildEntity>(List<TEntity> entities, Expression<Func<TEntity, List<TChildEntity>>> includeExpression, DeleteNavOptions? deleteNavOptions = null) where TChildEntity : class, IEntity, new();

        Task<bool> DeleteByIdAsync(TKey id, bool isSoftDelete = true, CancellationToken cancellationToken = default);

        Task<bool> DeleteByIdAsync(List<TKey> ids, bool isSoftDelete = true, CancellationToken cancellationToken = default);

        #endregion
    }
}
