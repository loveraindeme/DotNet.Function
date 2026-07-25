using DotNet.EFCore.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DotNet.EFCore.DbMigrator
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(
                    connectionString: configuration.GetConnectionString("Default"),
                    serverVersion: MySqlServerVersion.LatestSupportedServerVersion,
                    mySqlOptionsAction: optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(AppDbContextFactory).Assembly.GetName().Name
                ));

            return new AppDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false);

            if (!string.IsNullOrWhiteSpace(environment))
            {
                builder.AddJsonFile($"appsettings.{environment}.json", optional: true);
            }

            return builder.AddEnvironmentVariables().Build();
        }
    }
}
