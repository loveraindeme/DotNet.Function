using SugarSqlCore.Abstraction;
using SugarSqlCore.Abstraction.Entities;

namespace DotNet.SugarSqlCore.Repositories
{
    public interface IBaseRepository<TEntity> : ISugarSqlRepository<TEntity> where TEntity : class, IEntity, new()
    {
        string GenerateSortExpression(string sortField, string sortType, Type type);

        string GenerateSortExpression(string sortField, string sortType);
    }

    public interface IBaseRepository<TEntity, TKey> : IBaseRepository<TEntity>, ISugarSqlRepository<TEntity, TKey> where TEntity : class, IEntity, new()
    {

    }
}
