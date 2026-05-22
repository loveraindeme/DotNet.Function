namespace DotNet.Ftp.Contracts
{
    public class FtpOperationResult
    {
        public bool IsSuccess => Code == FtpOperationResultCode.None;

        public FtpOperationResultCode Code { get; set; } = FtpOperationResultCode.None;

        public string Message { get; set; } = string.Empty;

        public static FtpOperationResult Success(FtpOperationResultCode code = FtpOperationResultCode.None, string message = "")
        {
            return new FtpOperationResult
            {
                Code = code,
                Message = message
            };
        }

        public static FtpOperationResult Failed(FtpOperationResultCode code, string message = "")
        {
            return new FtpOperationResult
            {
                Code = code,
                Message = message
            };
        }
    }

    public class FtpOperationResult<T> : FtpOperationResult
    {
        public T? Data { get; set; }

        public static FtpOperationResult<T> Success(T? data, FtpOperationResultCode code = FtpOperationResultCode.None, string message = "")
        {
            return new FtpOperationResult<T>
            {
                Code = code,
                Message = message,
                Data = data
            };
        }

        public static FtpOperationResult<T> Failed(FtpOperationResultCode code, string message = "", T? data = default)
        {
            return new FtpOperationResult<T>
            {
                Code = code,
                Message = message,
                Data = data
            };
        }
    }
}
