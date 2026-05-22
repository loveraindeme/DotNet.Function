using DotNet.Ftp.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.Common.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FtpController : Controller
    {
        private readonly FtpFileServiceBuilder _ftpFileServiceBuilder;

        public FtpController(FtpFileServiceBuilder ftpFileServiceBuilder)
        {
            _ftpFileServiceBuilder = ftpFileServiceBuilder;
        }

        [HttpGet("filenames")]
        public async Task<List<string>> GetFileNamesAsync([FromQuery] string filePathDirectory)
        {
            var fileNames = new List<string>();
            await using var ftpFileService = await _ftpFileServiceBuilder.CreateAsync();
            var fileGetResult = await ftpFileService.GetFilesAsync(filePathDirectory);
            if (fileGetResult.IsSuccess && fileGetResult.Data != null)
            {
                fileNames = fileGetResult.Data.Select(f => f.Name).ToList();
            }
            return fileNames;
        }

        [HttpPost("exist")]
        public async Task<bool> ExistAsync([FromQuery] string filePath)
        {
            var result = false;
            await using var ftpFileService = await _ftpFileServiceBuilder.CreateAsync();
            var fileGetResult = await ftpFileService.ExistAsync(filePath);
            if (fileGetResult.IsSuccess)
            {
                result = fileGetResult.Data;
            }
            return result;
        }
    }
}
