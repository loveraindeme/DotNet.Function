using DotNet.Ftp.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace DotNet.Ftp
{
    public static class FtpExtension
    {
        public static IServiceCollection AddFluentFtp(this IServiceCollection services)
        {
            services.AddTransient<AppSettingsFtpConnectionOptionsProvider>();
            services.AddTransient<IFtpConnectionOptionsProvider, AppSettingsFtpConnectionOptionsProvider>();
            services.AddTransient<FtpFileServiceBuilder, DefaultFtpFileServiceBuilder>();
            return services;
        }
    }
}
