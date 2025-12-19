using AdminLine.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace AdminLine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ITripService _tripService;

        public WeatherForecastController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> gettrip()
        {
            await _tripService.getallTrip();
            return Ok();
        }
    }
}
