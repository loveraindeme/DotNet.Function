using Microsoft.AspNetCore.Mvc;

namespace DotNet.OpenApi.Controllers
{
    [ApiController]
    [EndpointGroupName("external")]
    [Route("api/[controller]")]
    public class ExternalController : Controller
    {
        public ExternalController()
        {

        }

        [HttpGet("random-number")]
        [EndpointSummary("Get random number.")]
        [EndpointDescription("Generates a random number within the specified range.")]
        public int? GetRandomNumber(int minNumber, int maxNumber)
        {
            if (minNumber >= maxNumber)
            {
                return null;
            }
            return new Random().Next(minNumber, maxNumber);
        }
    }
}
