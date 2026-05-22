using DotNet.Ftp.Contracts;
using FluentFTP;
using FluentFTP.Exceptions;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text.RegularExpressions;

namespace DotNet.Ftp
{
    public class FtpFileService : IFtpFileService
    {
        private readonly FtpConnectionOptions _connectionOptions;
        private readonly ILogger<FtpFileService> _logger;
        private AsyncFtpClient? _ftpClient;

        public bool IsAuthenticated { get; private set; }

        public FtpFileService(FtpConnectionOptions connectionOptions,
            ILogger<FtpFileService> logger)
        {
            _connectionOptions = connectionOptions;
            _logger = logger;
        }

        /// <summary>
        /// 判断FTP文件是否存在
        /// </summary>
        /// <param name="ftpFilePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FtpOperationResult<bool>> ExistAsync(string ftpFilePath, CancellationToken cancellationToken = default)
        {
            var authResult = await GetAuthenticatedClientAsync(cancellationToken);
            if (!authResult.IsSuccess || authResult.Data == null)
            {
                return FtpOperationResult<bool>.Failed(
                    FtpOperationResultCode.AuthError,
                    authResult.Message,
                    false);
            }
            var ftpClient = authResult.Data;
            try
            {
                var isExist = await ftpClient.FileExists(ftpFilePath, cancellationToken);
                return FtpOperationResult<bool>.Success(isExist, FtpOperationResultCode.None);
            }
            catch (Exception ex)
            {
                if (IsInvalidateConnection(ex))
                {
                    await InvalidateClientAsync();
                }
                _logger.LogError(ex, "FTP判断文件是否存在失败，FTP文件：{filePath}", ftpFilePath);
                return FtpOperationResult<bool>.Failed(FtpOperationResultCode.FileExistError, ex.Message, false);
            }
        }

        /// <summary>
        /// 获取FTP文件服务器中指定的文件目录列表
        /// </summary>
        /// <param name="ftpFileDirectory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FtpOperationResult<List<FtpListItem>>> GetDirectoriesAsync(string ftpFileDirectory, CancellationToken cancellationToken = default)
        {
            var fileDirectorys = new List<FtpListItem>();
            var authResult = await GetAuthenticatedClientAsync(cancellationToken);
            if (!authResult.IsSuccess || authResult.Data == null)
            {
                return FtpOperationResult<List<FtpListItem>>.Failed(
                    FtpOperationResultCode.AuthError,
                    authResult.Message,
                    fileDirectorys);
            }
            var ftpClient = authResult.Data;
            try
            {
                if (await ftpClient.DirectoryExists(ftpFileDirectory, cancellationToken))
                {
                    var stores = await ftpClient.GetListing(ftpFileDirectory, cancellationToken);
                    foreach (var store in stores)
                    {
                        if (store.Type == FtpObjectType.Directory)
                        {
                            fileDirectorys.Add(store);
                        }
                    }
                }
                _logger.LogInformation("FTP获取目录成功，FTP目录：{directory}，子目录数量：{count}", ftpFileDirectory, fileDirectorys.Count);
                return FtpOperationResult<List<FtpListItem>>.Success(fileDirectorys, FtpOperationResultCode.None);
            }
            catch (Exception ex)
            {
                if (IsInvalidateConnection(ex))
                {
                    await InvalidateClientAsync();
                }
                _logger.LogError(ex, "FTP获取目录失败，FTP目录：{directory}", ftpFileDirectory);
                return FtpOperationResult<List<FtpListItem>>.Failed(FtpOperationResultCode.DirectoriesGetError, ex.Message, fileDirectorys);
            }
        }

        /// <summary>
        /// 获取FTP文件服务器中指定目录下的文件列表
        /// </summary>
        /// <param name="ftpFileDirectory"></param>
        /// <param name="searchPattern"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FtpOperationResult<List<FtpListItem>>> GetFilesAsync(string ftpFileDirectory, string searchPattern = "*.txt", CancellationToken cancellationToken = default)
        {
            var files = new List<FtpListItem>();
            var authResult = await GetAuthenticatedClientAsync(cancellationToken);
            if (!authResult.IsSuccess || authResult.Data == null)
            {
                return FtpOperationResult<List<FtpListItem>>.Failed(
                    FtpOperationResultCode.AuthError,
                    authResult.Message,
                    files);
            }
            var ftpClient = authResult.Data;
            try
            {
                if (await ftpClient.DirectoryExists(ftpFileDirectory, cancellationToken))
                {
                    var stores = await ftpClient.GetListing(ftpFileDirectory, cancellationToken);
                    foreach (var store in stores)
                    {
                        if (store.Type != FtpObjectType.File)
                        {
                            continue;
                        }

                        if (!IsMatchPattern(store.Name, searchPattern))
                        {
                            continue;
                        }

                        files.Add(store);
                    }
                }
                _logger.LogInformation("FTP获取文件列表成功，FTP目录：{directory}，匹配规则：{pattern}，文件数量：{count}", ftpFileDirectory, searchPattern, files.Count);
                return FtpOperationResult<List<FtpListItem>>.Success(files, FtpOperationResultCode.None);
            }
            catch (Exception ex)
            {
                if (IsInvalidateConnection(ex))
                {
                    await InvalidateClientAsync();
                }
                _logger.LogError(ex, "FTP获取文件列表失败，FTP目录：{directory}，匹配规则：{pattern}", ftpFileDirectory, searchPattern);
                return FtpOperationResult<List<FtpListItem>>.Failed(FtpOperationResultCode.DirectoriesGetError, ex.Message, files);
            }
        }

        /// <summary>
        /// 下载FTP文件服务器中指定文件
        /// </summary>
        /// <param name="ftpFilePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FtpOperationResult<byte[]>> DownloadFileAsync(string ftpFilePath, CancellationToken cancellationToken = default)
        {
            var authResult = await GetAuthenticatedClientAsync(cancellationToken);
            if (!authResult.IsSuccess || authResult.Data == null)
            {
                return FtpOperationResult<byte[]>.Failed(
                    FtpOperationResultCode.AuthError,
                    authResult.Message,
                    []);
            }
            var ftpClient = authResult.Data;
            try
            {
                if (!await ftpClient.FileExists(ftpFilePath, cancellationToken))
                {
                    _logger.LogWarning("FTP下载文件失败，文件不存在，FTP文件：{filePath}", ftpFilePath);
                    return FtpOperationResult<byte[]>.Failed(FtpOperationResultCode.FileDownloadError, "文件不存在", []);
                }

                var fileBytes = await ftpClient.DownloadBytes(ftpFilePath, cancellationToken);
                _logger.LogInformation("FTP下载文件成功，FTP文件：{filePath}，字节数：{length}", ftpFilePath, fileBytes.Length);
                return FtpOperationResult<byte[]>.Success(fileBytes, FtpOperationResultCode.None);
            }
            catch (Exception ex)
            {
                if (IsInvalidateConnection(ex))
                {
                    await InvalidateClientAsync();
                }
                _logger.LogError(ex, "FTP下载文件失败，FTP文件：{filePath}", ftpFilePath);
                return FtpOperationResult<byte[]>.Failed(FtpOperationResultCode.FileDownloadError, ex.Message, []);
            }
        }

        /// <summary>
        /// 上传文件到FTP文件服务器
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="ftpFilePath"></param>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FtpOperationResult> UploadFileAsync(Stream fileStream, string ftpFilePath, string fileName, CancellationToken cancellationToken = default)
        {
            var authResult = await GetAuthenticatedClientAsync(cancellationToken);
            if (!authResult.IsSuccess || authResult.Data == null)
            {
                return FtpOperationResult.Failed(FtpOperationResultCode.AuthError, authResult.Message);
            }
            var ftpClient = authResult.Data;
            try
            {
                string fileFullPath = $"{ftpFilePath}/{fileName}";
                if (!await ftpClient.DirectoryExists(ftpFilePath, cancellationToken))
                {
                    await ftpClient.CreateDirectory(ftpFilePath, cancellationToken);
                }
                if (await ftpClient.FileExists(fileFullPath, cancellationToken))
                {
                    await ftpClient.DeleteFile(fileFullPath, cancellationToken);
                }
                if (fileStream.CanSeek)
                {
                    fileStream.Position = 0;
                }
                var ftpStatus = await ftpClient.UploadStream(fileStream, fileFullPath, token: cancellationToken);
                if (ftpStatus == FtpStatus.Failed)
                {
                    _logger.LogWarning("FTP上传文件失败，FTP目录：{path}，文件名：{fileName}", ftpFilePath, fileName);
                    return FtpOperationResult.Failed(FtpOperationResultCode.FileUploadError, "上传文件失败");
                }
                _logger.LogInformation("FTP上传文件成功，FTP目录：{path}，文件名：{fileName}", ftpFilePath, fileName);
            }
            catch (Exception ex)
            {
                if (IsInvalidateConnection(ex))
                {
                    await InvalidateClientAsync();
                }
                _logger.LogError(ex, "FTP上传文件异常，FTP目录：{path}，文件名：{fileName}", ftpFilePath, fileName);
                return FtpOperationResult.Failed(FtpOperationResultCode.FileUploadError, ex.Message);
            }
            return FtpOperationResult.Success(FtpOperationResultCode.None);
        }

        /// <summary>
        ///下载FTP文件服务器中指定的文件目录
        /// </summary>
        /// <param name="fileDirectory"></param>
        /// <param name="ftpFileDirectory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FtpOperationResult> DownloadFileDirectoryAsync(string fileDirectory, string ftpFileDirectory, CancellationToken cancellationToken = default)
        {
            var authResult = await GetAuthenticatedClientAsync(cancellationToken);
            if (!authResult.IsSuccess || authResult.Data == null)
            {
                return FtpOperationResult.Failed(FtpOperationResultCode.AuthError, authResult.Message);
            }
            var ftpClient = authResult.Data;
            try
            {
                if (await ftpClient.DirectoryExists(ftpFileDirectory, cancellationToken))
                {
                    await ftpClient.DownloadDirectory(fileDirectory, ftpFileDirectory, token: cancellationToken);
                }
                _logger.LogInformation("FTP下载目录成功，FTP目录：{remoteDirectory}，本地目录：{localDirectory}", ftpFileDirectory, fileDirectory);
                return FtpOperationResult.Success(FtpOperationResultCode.None);
            }
            catch (Exception ex)
            {
                if (IsInvalidateConnection(ex))
                {
                    await InvalidateClientAsync();
                }
                _logger.LogError(ex, "FTP下载目录失败，FTP目录：{remoteDirectory}，本地目录：{localDirectory}", ftpFileDirectory, fileDirectory);
                return FtpOperationResult.Failed(FtpOperationResultCode.DirectoryDownloadError, ex.Message);
            }
        }

        /// <summary>
        /// 删除FTP文件服务器中指定的文件
        /// </summary>
        /// <param name="ftpFilePath"></param>
        /// <param name="fileName"></param>
        /// <param name="isDeleteDirectory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FtpOperationResult> DeleteFileAsync(string ftpFilePath, string fileName, bool isDeleteDirectory = false, CancellationToken cancellationToken = default)
        {
            var authResult = await GetAuthenticatedClientAsync(cancellationToken);
            if (!authResult.IsSuccess || authResult.Data == null)
            {
                return FtpOperationResult.Failed(FtpOperationResultCode.AuthError, authResult.Message);
            }
            var ftpClient = authResult.Data;
            try
            {
                string fileFullPath = $"{ftpFilePath}/{fileName}";
                if (await ftpClient.FileExists(fileFullPath, cancellationToken))
                {
                    await ftpClient.DeleteFile(fileFullPath, cancellationToken);
                }
                if (isDeleteDirectory && await ftpClient.DirectoryExists(ftpFilePath, cancellationToken))
                {
                    var files = await ftpClient.GetListing(ftpFilePath, FtpListOption.Size, cancellationToken);
                    if (files.Length == 0)
                    {
                        await ftpClient.DeleteDirectory(ftpFilePath, cancellationToken);
                    }
                }
                _logger.LogInformation("FTP删除文件成功，FTP目录：{path}，FTP文件名：{fileName}", ftpFilePath, fileName);
                return FtpOperationResult.Success(FtpOperationResultCode.None);
            }
            catch (Exception ex)
            {
                if (IsInvalidateConnection(ex))
                {
                    await InvalidateClientAsync();
                }
                _logger.LogError(ex, "FTP删除文件失败，FTP目录：{path}，FTP文件名：{fileName}", ftpFilePath, fileName);
                return FtpOperationResult.Failed(FtpOperationResultCode.FileDeleteError, ex.Message);
            }
        }

        /// <summary>
        /// 删除FTP文件服务器中指定的文件目录
        /// </summary>
        /// <param name="ftpFileDirectory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FtpOperationResult> DeleteFileDirectoryAsync(string ftpFileDirectory, CancellationToken cancellationToken = default)
        {
            var authResult = await GetAuthenticatedClientAsync(cancellationToken);
            if (!authResult.IsSuccess || authResult.Data == null)
            {
                return FtpOperationResult.Failed(FtpOperationResultCode.AuthError, authResult.Message);
            }
            var ftpClient = authResult.Data;
            try
            {
                if (await ftpClient.DirectoryExists(ftpFileDirectory, cancellationToken))
                {
                    await ftpClient.DeleteDirectory(ftpFileDirectory, cancellationToken);
                }
                _logger.LogInformation("FTP删除目录成功，FTP目录：{directory}", ftpFileDirectory);
                return FtpOperationResult.Success(FtpOperationResultCode.None);
            }
            catch (Exception ex)
            {
                if (IsInvalidateConnection(ex))
                {
                    await InvalidateClientAsync();
                }
                _logger.LogError(ex, "FTP删除目录失败，FTP目录：{directory}", ftpFileDirectory);
                return FtpOperationResult.Failed(FtpOperationResultCode.DirectoryDeleteError, ex.Message);
            }
        }

        /// <summary>
        /// FTP认证
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FtpOperationResult> AuthenticatedAsync(CancellationToken cancellationToken = default)
        {
            var authResult = await GetAuthenticatedClientAsync(cancellationToken);
            if (!authResult.IsSuccess || authResult.Data == null)
            {
                return FtpOperationResult.Failed(FtpOperationResultCode.AuthError, authResult.Message);
            }
            _logger.LogInformation("FTP认证成功，地址：{server}", _connectionOptions.Server);
            return FtpOperationResult.Success();
        }

        public async ValueTask DisposeAsync()
        {
            await InvalidateClientAsync();
        }

        private AsyncFtpClient CreateClient()
        {
            return new AsyncFtpClient(
                _connectionOptions.Host,
                _connectionOptions.FtpAccount,
                _connectionOptions.FtpPassword,
                _connectionOptions.Port);
        }

        private async Task<FtpOperationResult<AsyncFtpClient>> GetAuthenticatedClientAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_ftpClient == null)
                {
                    _ftpClient = CreateClient();
                }
                if (!_ftpClient.IsConnected || !IsAuthenticated)
                {
                    await _ftpClient.AutoConnect(cancellationToken);
                    IsAuthenticated = true;
                }
                return FtpOperationResult<AsyncFtpClient>.Success(_ftpClient, FtpOperationResultCode.None);
            }
            catch (FtpAuthenticationException ex)
            {
                await InvalidateClientAsync();
                _logger.LogError(ex, "FTP认证失败，认证地址：{server}", _connectionOptions.Server);
                return FtpOperationResult<AsyncFtpClient>.Failed(FtpOperationResultCode.AuthError, "FTP认证失败");
            }
            catch (Exception ex)
            {
                await InvalidateClientAsync();
                _logger.LogError(ex, "FTP连接失败，连接地址：{server}", _connectionOptions.Server);
                return FtpOperationResult<AsyncFtpClient>.Failed(FtpOperationResultCode.AuthError, "FTP连接失败");
            }
        }

        private async Task InvalidateClientAsync()
        {
            IsAuthenticated = false;
            if (_ftpClient == null)
            {
                return;
            }
            try
            {
                await _ftpClient.Disconnect();
            }
            catch
            {

            }
            finally
            {
                await _ftpClient.DisposeAsync();
                _ftpClient = null;
            }
        }

        private static bool IsMatchPattern(string fileName, string searchPattern)
        {
            if (string.IsNullOrWhiteSpace(searchPattern) || searchPattern == "*")
            {
                return true;
            }

            var regexPattern = "^" + Regex.Escape(searchPattern)
                .Replace("\\*", ".*")
                .Replace("\\?", ".") + "$";

            return Regex.IsMatch(fileName, regexPattern, RegexOptions.IgnoreCase);
        }

        private static bool IsInvalidateConnection(Exception exception)
        {
            if (exception is FtpAuthenticationException ||
                exception is TimeoutException ||
                exception is SocketException ||
                exception is IOException ||
                exception is ObjectDisposedException ||
                exception is AuthenticationException ||
                exception is OperationCanceledException)
            {
                return true;
            }

            return false;
        }
    }
}
