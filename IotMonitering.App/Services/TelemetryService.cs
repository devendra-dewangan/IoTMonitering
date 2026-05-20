using IoTMonitering.Domain.Entity;
using IoTMonitoring.Migrations;

namespace IoTMonitoring.App.Services
{
    public class TelemetryService : ITelemetryService
    {
        private readonly ILogger<TelemetryService> _logger;
        public TelemetryService(ILogger<TelemetryService> logger)
        {
            _logger = logger;
        }
        public Task AddTelemetry(Telemetry dto)
        {
            _logger.LogInformation("Adding telemetry for device {DeviceId} with temperature {Temperature} and humidity {Humidity} at {Timestamp}",
                dto.DeviceId, dto.Temperature, dto.Humidity, dto.Timestamp);
            return Task.CompletedTask;
        }
    }

    public interface ITelemetryService
    {
        Task AddTelemetry(Telemetry dto);
    }
}
