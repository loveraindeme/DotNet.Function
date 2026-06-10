using DotNet.OpenApi.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.OpenApi.Controllers
{
    [ApiController]
    //[EndpointGroupName("third")]
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        public RegisterController() 
        {
        
        }

        [HttpPost("register-ball")]
        public void RegisterBallGames(Ball ball)
        {

        }
    }
}
