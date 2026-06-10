using Microsoft.AspNetCore.Mvc;

namespace DotNet.OpenApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonController : Controller
    {
        public CommonController()
        {

        }

        [HttpGet("current-time")]
        [EndpointSummary("Get current time.")]
        [EndpointDescription("Returns the current server time in UTC.")]
        public DateTime GetCurrentTime()
        {
            return DateTime.UtcNow;
        }
    }
}
