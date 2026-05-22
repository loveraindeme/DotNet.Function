using DotNet.Ftp.Contracts;
using Microsoft.Extensions.Logging;

namespace DotNet.Ftp
{
    public class DefaultFtpFileServiceBuilder : FtpFileServiceBuilder
    {
        private readonly IFtpConnectionOptionsProvider _connectionOptionsProvider;
        private readonly ILoggerFactory _loggerFactory;

        public DefaultFtpFileServiceBuilder(
            IFtpConnectionOptionsProvider connectionOptionsProvider,
            ILoggerFactory loggerFactory) : base(connectionOptionsProvider, loggerFactory)
        {
            _connectionOptionsProvider = connectionOptionsProvider;
            _loggerFactory = loggerFactory;
        }

        private DefaultFtpFileServiceBuilder(
            IFtpConnectionOptionsProvider connectionOptionsProvider,
            ILoggerFactory loggerFactory,
            string? account,
            string? password,
            string? server) : base(connectionOptionsProvider, loggerFactory, account, password, server)
        {
            _connectionOptionsProvider = connectionOptionsProvider;
            _loggerFactory = loggerFactory;
        }

        protected override FtpFileServiceBuilder Build(string? account, string? password, string? server)
        {
            return new DefaultFtpFileServiceBuilder(_connectionOptionsProvider, _loggerFactory, account, password, server);
        }
    }
}
