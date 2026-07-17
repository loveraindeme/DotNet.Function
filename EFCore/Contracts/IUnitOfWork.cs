using System.Data;

namespace EFCore.Contracts
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        bool HasActiveTransaction { get; }

        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        Task BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default);

        void Commit();

        Task CommitAsync(CancellationToken cancellationToken = default);

        void Rollback();

        Task RollbackAsync(CancellationToken cancellationToken = default);

        void ExecuteInTransaction(
            Action action,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        TResult ExecuteInTransaction<TResult>(
            Func<TResult> action,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> action,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default);

        Task<TResult> ExecuteInTransactionAsync<TResult>(
            Func<CancellationToken, Task<TResult>> action,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default);

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
