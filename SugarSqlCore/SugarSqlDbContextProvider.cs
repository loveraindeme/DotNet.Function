using Microsoft.Extensions.DependencyInjection;
using SugarSqlCore.Abstraction;

namespace SugarSqlCore
{
    public class SugarSqlDbContextProvider<TDbContext> : ISugarSqlDbContextProvider<TDbContext> where TDbContext : ISugarSqlDbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public SugarSqlDbContextProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TDbContext GetDbContext()
        {
            var dbContext = (TDbContext)_serviceProvider.GetRequiredService<ISugarSqlDbContext>();
            return dbContext;
        }
    }
}
