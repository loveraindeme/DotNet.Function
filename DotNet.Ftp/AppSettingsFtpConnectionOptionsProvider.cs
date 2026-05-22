using DotNet.Ftp.Contracts;
using Microsoft.Extensions.Configuration;

namespace DotNet.Ftp
{
    public class AppSettingsFtpConnectionOptionsProvider : IFtpConnectionOptionsProvider
    {
        private readonly IConfiguration _configuration;

        public AppSettingsFtpConnectionOptionsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<FtpConnectionOptions> GetOptionsAsync(CancellationToken cancellationToken = default)
        {
            var section = _configuration.GetSection(FtpConnectionOptions.FtpConnectionOption);
            var account = section["Account"] ?? string.Empty;
            var password = section["Password"] ?? string.Empty;
            var server = section["Server"] ?? string.Empty;

            return Task.FromResult(new FtpConnectionOptions(account, password, server));
        }
    }
}
