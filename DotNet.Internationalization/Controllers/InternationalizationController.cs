using DotNet.Internationalization.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace DotNet.Internationalization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternationalizationController : Controller
    {
        private readonly IStringLocalizer<MultiLanguage> _stringLocalizer;

        public InternationalizationController(IStringLocalizer<MultiLanguage> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet("method")]
        public string GetByMethod(string localizationKey)
        {
            return _stringLocalizer.GetString(localizationKey);
        }

        [HttpGet("index")]
        public string GetByIndex(string localizationKey)
        {
            return _stringLocalizer[localizationKey];
        }

        [HttpGet("all-culture")]
        public IEnumerable<string> GetAllCulture()
        {
            var cultureNames = CultureInfo.GetCultures(CultureTypes.AllCultures).Select(culture => culture.Name);
            return cultureNames;
        }

        [HttpPost("name")]
        public string CreateName(DataAnnotationsTestDto input)
        {
            // logic to create name
            return input.Name;
        }
    }
}
