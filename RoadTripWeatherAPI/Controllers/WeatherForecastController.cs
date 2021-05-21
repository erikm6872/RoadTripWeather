using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RoadTripWeatherAPI.Repositories;
using System.Threading.Tasks;

namespace RoadTripWeatherAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class WeatherForecastController : ControllerBase
    {
        private string testOrigin = "Minneapolis, MN";
        private string testDest = "San Francisco, CA";

        private readonly ILogger<WeatherForecastController> _logger;
        private IConfiguration _config;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet]
        [Route("directions/weather")]
        public async Task<IActionResult> Get(string origin, string destination)
        {
            var dataHandler = new DataHandler(_config);
            var ret = await dataHandler.GetHourlyWeatherForRoute(origin, destination);

            return Ok(ret);
        }

        [HttpGet]
        [Route("directions/weather/test")]
        public async Task<IActionResult> Get()
        {
            var dataHandler = new DataHandler(_config);
            var ret = await dataHandler.GetHourlyWeatherForRoute(testOrigin, testDest);

            return Ok(ret);
        }

        [HttpGet]
        [Route("heartbeat")]
        public IActionResult GetHeartbeat()
        {
            return Ok();
        }
    }
}
