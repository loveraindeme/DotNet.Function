using SqlSugar;
using SugarSqlCore.Abstraction;

namespace SugarSqlCore
{
    public class SugarSqlDbConnectionBuilder : ISugarSqlDbConnectionBuilder
    {
        public SugarSqlDbConnectionBuilder() 
        {
        
        }

        public ConnectionConfig Build(Action<ConnectionConfig>? action = null)
        {
            var connectionConfig = new ConnectionConfig()
            {

            };
            action?.Invoke(connectionConfig);
            return connectionConfig;
        }
    }
}
