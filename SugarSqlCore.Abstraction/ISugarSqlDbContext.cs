using SqlSugar;

namespace SugarSqlCore.Abstraction
{
    public interface ISugarSqlDbContext
    {
        public ISqlSugarClient SugarSqlClient { get; }
    }
}
