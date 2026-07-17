using EFCore.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace EFCore
{
    public class EFCoreUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private IDbContextTransaction? _currentTransaction;
        private int _transactionDepth;
        private bool _disposed;

        public EFCoreUnitOfWork(IEFCoreDbContext dbContext)
        {
            _dbContext = dbContext as DbContext
                ?? throw new ArgumentException(
                    $"{nameof(IEFCoreDbContext)} must be implemented by {nameof(DbContext)}.",
                    nameof(dbContext));
        }

        public bool HasActiveTransaction => _currentTransaction != null;

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            ThrowIfDisposed();
            if (_currentTransaction != null)
            {
                _transactionDepth++;
                return;
            }

            _currentTransaction = _dbContext.Database.BeginTransaction(isolationLevel);
            _transactionDepth = 1;
        }

        public async Task BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (_currentTransaction != null)
            {
                _transactionDepth++;
                return;
            }

            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            _transactionDepth = 1;
        }

        public void Commit()
        {
            ThrowIfDisposed();
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No active transaction exists.");
            }

            if (--_transactionDepth > 0)
            {
                return;
            }

            try
            {
                _dbContext.SaveChanges();
                _currentTransaction!.Commit();
            }
            catch
            {
                _currentTransaction?.Rollback();
                throw;
            }
            finally
            {
                DisposeTransaction();
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No active transaction exists.");
            }

            if (--_transactionDepth > 0)
            {
                return;
            }

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                await _currentTransaction!.CommitAsync(cancellationToken);
            }
            catch
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(cancellationToken);
                }
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public void Rollback()
        {
            ThrowIfDisposed();
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                DisposeTransaction();
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(cancellationToken);
                }
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public void ExecuteInTransaction(
            Action action,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            ArgumentNullException.ThrowIfNull(action);

            BeginTransaction(isolationLevel);
            try
            {
                action();
                Commit();
            }
            catch
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Rollback();
                }
                DisposeTransaction();
                throw;
            }
        }

        public TResult ExecuteInTransaction<TResult>(
            Func<TResult> action,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            ArgumentNullException.ThrowIfNull(action);

            BeginTransaction(isolationLevel);
            try
            {
                var result = action();
                Commit();
                return result;
            }
            catch
            {
                TryRollback();
                DisposeTransaction();
                throw;
            }
        }

        public async Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> action,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action);

            await BeginTransactionAsync(isolationLevel, cancellationToken);
            try
            {
                await action(cancellationToken);
                await CommitAsync(cancellationToken);
            }
            catch
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(cancellationToken);
                }
                await DisposeTransactionAsync();
                throw;
            }
        }

        public async Task<TResult> ExecuteInTransactionAsync<TResult>(
            Func<CancellationToken, Task<TResult>> action,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action);

            await BeginTransactionAsync(isolationLevel, cancellationToken);
            try
            {
                var result = await action(cancellationToken);
                await CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(cancellationToken);
                }
                await DisposeTransactionAsync();
                throw;
            }
        }

        public int SaveChanges()
        {
            ThrowIfDisposed();
            return _dbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            if (_currentTransaction != null)
            {
                TryRollback();
                DisposeTransaction();
            }

            _disposed = true;
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return;
            }

            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync(CancellationToken.None);
                await DisposeTransactionAsync();
            }

            _disposed = true;
            GC.SuppressFinalize(this);
        }

        private void TryRollback()
        {
            try
            {
                
            }
            catch
            {
                // 保留导致提交失败的原始异常。
            }
        }

        private void DisposeTransaction()
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
            _transactionDepth = 0;
        }

        private async ValueTask DisposeTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
            _transactionDepth = 0;
        }

        private void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
        }
    }
}
