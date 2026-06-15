using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.VersionControl.Controllers.v3
{
    [ApiController]
    [ApiVersion("3.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TemperatureController : ControllerBase
    {
        [HttpGet]
        public int GetCurrentTemperature()
        {
            var apiVersion = HttpContext.RequestedApiVersion?.ToString();
            return Random.Shared.Next(-25, 50);
        }
    }
}
