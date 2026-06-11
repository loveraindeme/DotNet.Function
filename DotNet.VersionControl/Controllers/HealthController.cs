using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.VersionControl.Controllers
{
    [ApiController]
    [ApiVersionNeutral] // 版本中立
    [Route("api/[controller]")] // 不接受任何版本
    //[Route("api/v{version:apiVersion}/[controller]")] // 接受任何有效的版本
    public class HealthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}
