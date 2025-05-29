using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherApiWrapper.Services;


namespace WeatherApiWrapper.Controllers
{
    //tells .NET this class handles API requests
    [ApiController]
    //Sets url to match the controller name
    [Route("[controller]")]

    // Defines a controller for handling weather-related requests
    public class WeatherController : ControllerBase
    {
        
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City is required.");

            var result = await _weatherService.GetWeatherAsync(city);

            if (result == null)
                return NotFound("Weather data not found.");

            return Ok(result);
        }
    }
}
