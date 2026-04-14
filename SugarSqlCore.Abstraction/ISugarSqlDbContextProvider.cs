namespace SugarSqlCore.Abstraction
{
    public interface ISugarSqlDbContextProvider<TDbContext> where TDbContext : ISugarSqlDbContext
    {
        public TDbContext GetDbContext();
    }
}
