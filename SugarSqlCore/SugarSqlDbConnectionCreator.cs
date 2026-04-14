using SqlSugar;
using SugarSqlCore.Abstraction;

namespace SugarSqlCore
{
    public class SugarSqlDbConnectionCreator : ISugarSqlDbConnectionCreator
    {
        public SugarSqlDbConnectionCreator() 
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
