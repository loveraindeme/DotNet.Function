using DotNet.FluentValidation.Dtos;
using DotNet.FluentValidation.Validator;
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

        [HttpPatch]
        public async Task UpdateAsync(ValidationManualDto input)
        {
            var validator = new ValidationManualValidator();
            var validationResult = validator.Validate(input);
            // handle validation result
            // logic to update
        }
    }
}
