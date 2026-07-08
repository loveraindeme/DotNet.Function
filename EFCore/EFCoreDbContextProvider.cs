using EFCore.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore
{
    public class EFCoreDbContextProvider<TDbContext> : IEFCoreDbContextProvider<TDbContext> where TDbContext : IEFCoreDbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public EFCoreDbContextProvider(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public TDbContext GetDbContext()
        {
            return (TDbContext)_serviceProvider.GetRequiredService<IEFCoreDbContext>();
        }
    }
}
