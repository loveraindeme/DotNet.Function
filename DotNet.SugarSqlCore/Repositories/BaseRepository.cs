using SugarSqlCore;
using SugarSqlCore.Abstraction;
using SugarSqlCore.Abstraction.Entities;
using System.Reflection;

namespace DotNet.SugarSqlCore.Repositories
{
    public abstract class BaseRepository<TEntity> : SugarSqlRepository<TEntity>, IBaseRepository<TEntity> where TEntity : class, IEntity, new()
    {
        public BaseRepository(ISugarSqlDbContextProvider<ISugarSqlDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public string GenerateSortExpression(string sortField, string sortType, Type type)
        {
            string sortExpression = string.Empty;
            if (IsValidField(sortField, type))
            {
                sortExpression = sortField;
                if (sortType != "asc")
                {
                    sortExpression += " desc";
                }
                else
                {
                    sortExpression += " asc";
                }
            }
            return sortExpression;
        }

        public string GenerateSortExpression(string sortField, string sortType)
        {
            string sortExpression = string.Empty;
            if (IsValidField(sortField))
            {
                sortExpression = sortField;
                if (sortType != "asc")
                {
                    sortExpression += " desc";
                }
                else
                {
                    sortExpression += " asc";
                }
            }
            return sortExpression;
        }

        private static bool IsValidField(string field, Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Equals(field))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsValidField(string field)
        {
            try
            {
                string property = GetDbContext().EntityMaintenance.GetDbColumnName<TEntity>(field);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }

    public abstract class BaseRepository<TEntity, TKey> : SugarSqlRepository<TEntity, TKey>, IBaseRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        public BaseRepository(ISugarSqlDbContextProvider<ISugarSqlDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public string GenerateSortExpression(string sortField, string sortType, Type type)
        {
            string sortExpression = string.Empty;
            if (IsValidField(sortField, type))
            {
                sortExpression = sortField;
                if (sortType != "asc")
                {
                    sortExpression += " desc";
                }
                else
                {
                    sortExpression += " asc";
                }
            }
            return sortExpression;
        }

        public string GenerateSortExpression(string sortField, string sortType)
        {
            string sortExpression = string.Empty;
            if (IsValidField(sortField))
            {
                sortExpression = sortField;
                if (sortType != "asc")
                {
                    sortExpression += " desc";
                }
                else
                {
                    sortExpression += " asc";
                }
            }
            return sortExpression;
        }

        private static bool IsValidField(string field, Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Equals(field))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsValidField(string field)
        {
            try
            {
                string property = GetDbContext().EntityMaintenance.GetDbColumnName<TEntity>(field);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
