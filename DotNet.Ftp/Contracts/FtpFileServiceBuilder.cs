using Microsoft.Extensions.Logging;

namespace DotNet.Ftp.Contracts
{
    public abstract class FtpFileServiceBuilder
    {
        private readonly IFtpConnectionOptionsProvider _ftpConnectionOptionsProvider;
        private readonly ILoggerFactory _loggerFactory;

        protected string? Account { get; }

        protected string? Password { get; }

        protected string? Server { get; }

        protected FtpFileServiceBuilder(
            IFtpConnectionOptionsProvider ftpConnectionOptionsProvider,
            ILoggerFactory loggerFactory,
            string? account = null,
            string? password = null,
            string? server = null)
        {
            _ftpConnectionOptionsProvider = ftpConnectionOptionsProvider;
            _loggerFactory = loggerFactory;
            Account = account;
            Password = password;
            Server = server;
        }

        public FtpFileServiceBuilder WithAuthentication(string account, string password)
        {
            return Build(account, password, Server);
        }

        public FtpFileServiceBuilder WithHost(string server)
        {
            return Build(Account, Password, server);
        }

        protected abstract FtpFileServiceBuilder Build(string? account, string? password, string? server);

        public async Task<IFtpFileService> CreateAsync(CancellationToken cancellationToken = default)
        {
            var sourceOptions = await _ftpConnectionOptionsProvider.GetOptionsAsync(cancellationToken);
            var options = new FtpConnectionOptions(
                string.IsNullOrWhiteSpace(Account) ? sourceOptions.FtpAccount : Account,
                string.IsNullOrWhiteSpace(Password) ? sourceOptions.FtpPassword : Password,
                string.IsNullOrWhiteSpace(Server) ? sourceOptions.Server : Server);
            return new FtpFileService(options, _loggerFactory.CreateLogger<FtpFileService>());
        }
    }
}
