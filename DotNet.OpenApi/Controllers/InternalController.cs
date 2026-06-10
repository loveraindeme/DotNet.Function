using Microsoft.AspNetCore.Mvc;

namespace DotNet.OpenApi.Controllers
{
    [ApiController]
    [EndpointGroupName("internal")]
    [Route("api/[controller]")]
    public class InternalController : Controller
    {
        public InternalController()
        {

        }

        [HttpPost("calculate-age")]
        [EndpointSummary("Get age.")]
        [EndpointDescription("Calculates the age based on the provided birthday.")]
        public int? CalculateAge(DateTime birthday)
        {
            if (birthday > DateTime.Now)
            {
                return null;
            }
            return DateTime.Now.Year - birthday.Year;
        }
    }
}
