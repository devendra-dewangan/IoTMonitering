using DeviceMock.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;


namespace DeviceMock.Clients
{
    internal class TelemtryHubClient : IClient
    {
        private readonly HubConnection _hubConnection;
        private readonly ILogger _logger;

        public TelemtryHubClient(HubConnection hubConnection, ILogger<TelemtryHubClient> logger)
        {
            _hubConnection = hubConnection;
            _logger = logger;
            _logger.LogInformation("[SignalR] Connected");
        }

        public bool IsDeviceRegistered(string deviceId)
        {
            return false;
        }

        public async Task SendTelemetryAsync(Telemetry data)
        {
            try
            {
                await _hubConnection.InvokeAsync("SendTelemetry", data);
                _logger.LogInformation($"[SignalR] {data.DeviceId} → Sent");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[SignalR ERROR] {ex.Message}");
            }
        }
    }
}
