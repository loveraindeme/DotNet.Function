using EFCore.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EFCore
{
    public class EFCoreUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public EFCoreUnitOfWork(IEFCoreDbContext dbContext)
        {
            _dbContext = (dbContext as DbContext)!;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
