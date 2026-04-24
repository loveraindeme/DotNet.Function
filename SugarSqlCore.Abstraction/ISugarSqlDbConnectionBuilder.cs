using SqlSugar;

namespace SugarSqlCore.Abstraction
{
    public interface ISugarSqlDbConnectionBuilder
    {
        ConnectionConfig Build(Action<ConnectionConfig>? action = null);
    }
}
