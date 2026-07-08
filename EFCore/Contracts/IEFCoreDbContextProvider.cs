namespace EFCore.Contracts
{
    public interface IEFCoreDbContextProvider<TDbContext> where TDbContext : IEFCoreDbContext
    {
        TDbContext GetDbContext();
    }
}
