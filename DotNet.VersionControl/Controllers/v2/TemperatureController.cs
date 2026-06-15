using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.VersionControl.Controllers.v2
{
    [ApiController]
    [ApiVersion(2.0, status: "Alpha")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TemperatureController : ControllerBase
    {
        [HttpGet]
        public int GetCurrentTemperature()
        {
            return Random.Shared.Next(-30, 50);
        }
    }
}
