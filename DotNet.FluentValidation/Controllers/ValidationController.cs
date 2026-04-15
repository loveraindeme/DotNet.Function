using DotNet.FluentValidation.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.FluentValidation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationController : Controller
    {
        public ValidationController() 
        {
        
        }

        [HttpPost]
        public async Task CreateAsync(ValidationTestDto input)
        {
            // logic to create
        }
    }
}
