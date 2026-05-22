namespace DotNet.Ftp
{
    internal static class FtpEndpointParser
    {
        public static bool TryParse(string server, out string host, out int port, out string error)
        {
            host = string.Empty;
            port = 21;
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(server))
            {
                error = "FTP的Server不能为空";
                return false;
            }

            var endpoint = server.Contains("://", StringComparison.Ordinal) ? server : $"ftp://{server}";
            if (!Uri.TryCreate(endpoint, UriKind.Absolute, out var uri))
            {
                error = $"FTP的Server格式无效：{server}";
                return false;
            }

            if (string.IsNullOrWhiteSpace(uri.Host))
            {
                error = $"FTP的Server解析失败：{server}";
                return false;
            }

            host = uri.Host;
            port = uri.IsDefaultPort ? 21 : uri.Port;
            return true;
        }
    }
}
