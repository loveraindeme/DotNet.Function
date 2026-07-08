using EFCore.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore
{
    public static class EFCoreExtensions
    {
        public static IServiceCollection AddEFCore<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsBuilder = null)
            where TDbContext : DbContext, IEFCoreDbContext
        {
            services.AddDbContext<TDbContext>(options =>
            {
                optionsBuilder?.Invoke(options);
            });
            services.AddScoped<IEFCoreDbContext>(sp => sp.GetRequiredService<TDbContext>());
            services.AddScoped<IUnitOfWork, EFCoreUnitOfWork>();
            services.AddTransient(typeof(IEFCoreDbContextProvider<>), typeof(EFCoreDbContextProvider<>));
            services.AddTransient(typeof(IEFCoreRepository<>), typeof(EFCoreRepository<>));
            services.AddTransient(typeof(IEFCoreRepository<,>), typeof(EFCoreRepository<,>));

            return services;
        }
    }
}
