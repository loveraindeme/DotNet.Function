namespace DotNet.Ftp.Contracts
{
    public interface IFtpConnectionOptionsProvider
    {
        Task<FtpConnectionOptions> GetOptionsAsync(CancellationToken cancellationToken = default);
    }
}
