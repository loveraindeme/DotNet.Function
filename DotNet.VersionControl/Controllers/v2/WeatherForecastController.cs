using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.VersionControl.Controllers.v2
{
    [ApiController]
    [ApiVersion(2.0, status: "Alpha")]
    [ApiVersion("3.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [MapToApiVersion(2.0, status: "Alpha")]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 10).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-30, 50),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet("current")]
        public WeatherForecast GetCurrent()
        {
            return new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = Random.Shared.Next(-30, 50),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };
        }
    }
}
