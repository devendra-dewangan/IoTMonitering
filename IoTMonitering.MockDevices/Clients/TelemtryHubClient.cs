using IoTMonitering.Domain.Entity;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Data.Common;


namespace DeviceMock.Clients
{
    internal class TelemtryHubClient : IClient
    {
        private readonly HubConnection _hubConnection;
        private readonly ILogger _logger;

        public TelemtryHubClient(HubConnection hubConnection, ILogger<TelemtryHubClient> logger)
        {
            _hubConnection = hubConnection;
            _logger = logger;;
            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                _logger.LogInformation($"{user}: {message}");
            });
            _hubConnection.StartAsync().Wait();

            _logger.LogInformation("Connected to SignalR Hub");
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
