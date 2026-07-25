using DotNet.EFCore.Database;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotNet.EFCore.DbMigrator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IDesignTimeDbContextFactory<AppDbContext>, AppDbContextFactory>();
                    services.AddHostedService<DbMigratorHostedService>();
                });
            return hostBuilder;
        }
    }
}
