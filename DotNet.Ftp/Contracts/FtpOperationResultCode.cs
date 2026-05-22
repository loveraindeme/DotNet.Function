namespace DotNet.Ftp.Contracts
{
    public enum FtpOperationResultCode
    {
        None = 0,

        /// <summary>
        /// 认证失败
        /// </summary>
        AuthError = 1,

        /// <summary>
        /// 文件存在判断失败
        /// </summary>
        FileExistError = 2,

        /// <summary>
        /// 文件目录列表获取失败
        /// </summary>
        DirectoriesGetError = 3,

        /// <summary>
        /// 文件上传失败
        /// </summary>
        FileUploadError = 4,

        /// <summary>
        /// 文件目录下载失败
        /// </summary>
        DirectoryDownloadError = 5,

        /// <summary>
        /// 文件下载失败
        /// </summary>
        FileDownloadError = 6,

        /// <summary>
        /// 文件删除失败
        /// </summary>
        FileDeleteError = 7,

        /// <summary>
        /// 文件目录删除失败
        /// </summary>
        DirectoryDeleteError = 8
    }
}
