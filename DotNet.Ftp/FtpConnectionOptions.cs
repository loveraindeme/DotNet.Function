namespace DotNet.Ftp
{
    public class FtpConnectionOptions
    {
        public const string FtpConnectionOption = "FTP";

        public string FtpAccount { get; }

        public string FtpPassword { get; }

        public string Host { get; }

        public string Server { get; }

        public int Port { get; }

        public FtpConnectionOptions(string ftpAccount, string ftpPassword, string server)
        {
            FtpAccount = ftpAccount;
            FtpPassword = ftpPassword;
            Server = server;
            if (!FtpEndpointParser.TryParse(server, out var host, out var port, out var error))
            {
                throw new ArgumentException(error, nameof(server));
            }
            Host = host;
            Port = port;
        }
    }
}
