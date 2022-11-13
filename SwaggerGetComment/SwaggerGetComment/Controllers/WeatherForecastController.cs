using Microsoft.AspNetCore.Mvc;

namespace SwaggerGetComment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Lindo método
        /// </summary>
        /// <remarks>
        /// Lindo método que trás a previsão tempo ;P
        /// 
        /// Ele busca isso lá do *SAP* tá?
        /// 
        /// Lá do *ACL* [Link para o ACL](https://dev.azure.com/CSCRZ/ACSDEVOPS/_git/CSACL/)
        /// 
        /// **Importante!** não faça isso, tá?
        /// </remarks>
        /// <response code="200">Tudo certo!</response>
        /// <response code="400">Mandou algo errado ai em!</response>
        /// <response code="500">Oops! Deu rum</response>
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}