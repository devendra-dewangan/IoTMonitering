using IoTMonitering.Domain.Entity;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace IoTMonitoring.Hubs
{
    public class TelemetryHub : Hub
    {
        private readonly ILogger<TelemetryHub> _logger;

        public TelemetryHub(ILogger<TelemetryHub> logger)
        {
            _logger = logger;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.Identity?.Name ?? "Unknown";
            await Clients.Caller.SendAsync("Connected", $"Welcome {userId}, you are now connected to TelemetryHub!");
            await base.OnConnectedAsync();
        }

        public async Task SendTelemetry(Telemetry data)
        {
            _logger.LogInformation($"Telemetry from: {JsonSerializer.Serialize(data)}");

            await Clients.All.SendAsync(
                "ReceiveTelemetry",
                data);
        }
    }
}
