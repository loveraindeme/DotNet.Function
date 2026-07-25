using DotNet.EFCore.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNet.EFCore.DbMigrator
{
    public class DbMigratorHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IDesignTimeDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly ILogger<DbMigratorHostedService> _logger;

        public DbMigratorHostedService(
            IHostApplicationLifetime hostApplicationLifetime,
            IDesignTimeDbContextFactory<AppDbContext> dbContextFactory,
            ILogger<DbMigratorHostedService> logger)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await using var dbContext = _dbContextFactory.CreateDbContext([]);

                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
                if (!pendingMigrations.Any())
                {
                    _logger.LogInformation("数据库已是最新版本，无需迁移");
                    return;
                }

                _logger.LogInformation("数据迁移开始，待应用的迁移：{Migrations}", string.Join(", ", pendingMigrations));
                await dbContext.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("数据迁移完成");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "数据迁移失败");
            }
            finally
            {
                _hostApplicationLifetime.StopApplication();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
