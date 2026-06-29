using Microsoft.AspNetCore.Mvc;

namespace DotNet.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public UserController()
        {

        }

        [PermissionAuthorize("user:list")]
        [HttpGet("users")]
        public Task<List<string>> GetAllUserListAsync()
        {
            return Task.FromResult(new List<string>(["rain", "linh"]));
        }

        [PermissionAuthorize("user:add")]
        [HttpPost]
        public Task CreateAsync()
        {
            return Task.CompletedTask;
        }
    }
}
