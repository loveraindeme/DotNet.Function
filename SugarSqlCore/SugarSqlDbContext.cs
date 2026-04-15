using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SqlSugar;
using SugarSqlCore.Abstraction;
using SugarSqlCore.Abstraction.Entities;

namespace SugarSqlCore
{
    public class SugarSqlDbContext : ISugarSqlDbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public ISqlSugarClient SugarSqlClient { get; private set; }

        public SugarSqlDbContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var dbConnectionCreator = _serviceProvider.GetRequiredService<ISugarSqlDbConnectionCreator>();
            var dbConnectionOptions = _serviceProvider.GetRequiredService<IOptions<DbConnectionOptions>>().Value;

            DbType dbType;
            switch (dbConnectionOptions.Type)
            {
                case "MySql":
                    dbType = DbType.MySql;
                    break;
                case "SqlServer":
                    dbType = DbType.SqlServer;
                    break;
                case "Sqlite":
                    dbType = DbType.Sqlite;
                    break;
                case "Oracle":
                    dbType = DbType.Oracle;
                    break;
                case "PostgreSQL":
                    dbType = DbType.PostgreSQL;
                    break;
                default:
                    dbType = DbType.MySql;
                    break;
            }

            SugarSqlClient = new SqlSugarClient(dbConnectionCreator.Build(options =>
            {
                options.DbType = dbType;
                options.ConnectionString = dbConnectionOptions.ConnectionString;
                options.IsAutoCloseConnection = true;
                options.InitKeyType = InitKeyType.Attribute;
                options.AopEvents = new AopEvents
                {
                    DataExecuting = (oldValue, entityInfo) =>
                    {
                        //插入事件
                        if (entityInfo.OperationType == DataFilterType.InsertByObject)
                        {
                            if (entityInfo.PropertyName.Equals(nameof(IEntity<Guid>.Id)))
                            {
                                if (Guid.Empty.Equals(oldValue))
                                {
                                    entityInfo.SetValue(Guid.NewGuid());
                                }
                            }
                            //if (entityInfo.PropertyName.Equals(nameof(ICreationAuditedEntity.CreationTime)))
                            //{
                            //    if (oldValue is null || DateTime.MinValue.Equals(oldValue))
                            //    {
                            //        entityInfo.SetValue(DateTime.Now);
                            //    }
                            //}
                            //if (entityInfo.PropertyName.Equals(nameof(ICreationAuditedEntity.CreatorId)))
                            //{
                            //    entityInfo.SetValue(CurrentUserAmbient.UserId);
                            //}
                            //行级别事件
                            if (entityInfo.EntityColumnInfo.IsPrimarykey)
                            {
                                var entity = entityInfo.EntityValue;
                                if (entity is ICreationAuditedEntity creationAuditedEntity)
                                {
                                    creationAuditedEntity.CreationTime = DateTime.Now;
                                    creationAuditedEntity.CreatorId = CurrentUserAmbient.UserId;
                                }
                            }
                        }
                        //更新事件
                        if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                        {
                            //行级别事件
                            if (entityInfo.EntityColumnInfo.IsPrimarykey)
                            {
                                var entity = entityInfo.EntityValue;
                                if (entity is IUpdationAuditedEntity updationAuditedEntity)
                                {
                                    updationAuditedEntity.LastModificationTime = DateTime.Now;
                                    updationAuditedEntity.LastModifierId = CurrentUserAmbient.UserId;
                                }
                            }
                        }
                    },
                    OnLogExecuting = (sql, param) =>
                    {
#if DEBUG
                        Console.WriteLine("\r\n" + "【SQL语句】：" + GetWholeSql(param, sql));
#endif
                    }
                };
            }), dbContext =>
            {
                dbContext.QueryFilter.AddTableFilter<IDeletionAuditedEntity>(entity => entity.IsDeleted == false);
            });
        }

        private static string GetWholeSql(SugarParameter[] paramArr, string sql)
        {
            foreach (var param in paramArr)
            {
                sql = sql.Replace(param.ParameterName, param.Value?.ToString());
            }
            return sql;
        }
    }
}
