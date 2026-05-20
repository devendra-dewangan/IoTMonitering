using IoTMonitering.Domain.Entity;
using IoTMonitoring.App.Services;
using IoTMonitoring.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoTMonitoring.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TelemetryController : ControllerBase
    {
        private ITelemetryService _telemetryService;

        public TelemetryController(ITelemetryService telemetryService)
        {
            _telemetryService = telemetryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTelemetry(TelemetryCreateDto dto)
        {

            var telemetry = new Telemetry
            {
                Temperature = dto.Temperature,
                Humidity = dto.Humidity,
                Timestamp = DateTime.UtcNow,
            };

            await _telemetryService.AddTelemetry(telemetry);

            return Ok("Telemetry recorded and broadcasted");
        }
    }
}
