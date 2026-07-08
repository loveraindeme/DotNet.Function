using Microsoft.EntityFrameworkCore;

namespace EFCore.Contracts
{
    public interface IEFCoreDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
