using SqlSugar;

namespace SugarSqlCore.Abstraction
{
    public interface ISugarSqlDbConnectionCreator
    {
        ConnectionConfig Build(Action<ConnectionConfig>? action = null);
    }
}
