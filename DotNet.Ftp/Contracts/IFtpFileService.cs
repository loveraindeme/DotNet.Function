using FluentFTP;

namespace DotNet.Ftp.Contracts
{
    public interface IFtpFileService : IAsyncDisposable
    {
        /// <summary>
        /// FTP认证状态
        /// </summary>
        public bool IsAuthenticated { get; }

        /// <summary>
        /// 判断FTP文件是否存在
        /// </summary>
        /// <param name="ftpFilePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<FtpOperationResult<bool>> ExistAsync(string ftpFilePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取FTP文件服务器中指定的文件目录列表
        /// </summary>
        /// <param name="ftpFileDirectory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<FtpOperationResult<List<FtpListItem>>> GetDirectoriesAsync(string ftpFileDirectory, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取FTP文件服务器中指定目录下的文件列表
        /// </summary>
        /// <param name="ftpFileDirectory"></param>
        /// <param name="searchPattern"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<FtpOperationResult<List<FtpListItem>>> GetFilesAsync(string ftpFileDirectory, string searchPattern = "*.txt", CancellationToken cancellationToken = default);

        /// <summary>
        /// 下载FTP文件服务器中指定文件
        /// </summary>
        /// <param name="ftpFilePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<FtpOperationResult<byte[]>> DownloadFileAsync(string ftpFilePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 上传文件到FTP文件服务器
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="ftpFilePath"></param>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<FtpOperationResult> UploadFileAsync(Stream fileStream, string ftpFilePath, string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        ///下载FTP文件服务器中指定的文件目录
        /// </summary>
        /// <param name="fileDirectory"></param>
        /// <param name="ftpFileDirectory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<FtpOperationResult> DownloadFileDirectoryAsync(string fileDirectory, string ftpFileDirectory, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除FTP文件服务器中指定的文件
        /// </summary>
        /// <param name="ftpFilePath"></param>
        /// <param name="fileName"></param>
        /// <param name="isDeleteDirectory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<FtpOperationResult> DeleteFileAsync(string ftpFilePath, string fileName, bool isDeleteDirectory = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除FTP文件服务器中指定的文件目录
        /// </summary>
        /// <param name="ftpFileDirectory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<FtpOperationResult> DeleteFileDirectoryAsync(string ftpFileDirectory, CancellationToken cancellationToken = default);

        /// <summary>
        /// FTP认证
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<FtpOperationResult> AuthenticatedAsync(CancellationToken cancellationToken = default);
    }
}
