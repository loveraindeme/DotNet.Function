using Microsoft.Extensions.DependencyInjection;
using SugarSqlCore.Abstraction;

namespace SugarSqlCore
{
    public static class SugarSqlExtension
    {
        public static void AddSugarSql(this IServiceCollection services, Action<DbConnectionOptions> configureOptions)
        {
            services.AddOptions();
            services.Configure(configureOptions);
            services.AddTransient<ISugarSqlDbConnectionCreator, SugarSqlDbConnectionCreator>();
            services.AddScoped<ISugarSqlDbContext, SugarSqlDbContext>();
            services.AddTransient(typeof(ISugarSqlDbContextProvider<>), typeof(SugarSqlDbContextProvider<>));
            services.AddTransient(typeof(ISugarSqlRepository<>), typeof(SugarSqlRepository<>));
            services.AddTransient(typeof(ISugarSqlRepository<,>), typeof(SugarSqlRepository<,>));
        }
    }
}
