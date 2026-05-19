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
            _hubConnection.On<string, string>("ReceiveMessage", (user, message) => _logger.LogInformation($"{user}: {message}"));
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                await _hubConnection.StartAsync();
                _logger.LogInformation("SignalR Hub Connection Started");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to SignalR Hub");
            }

            return false;
        }


        public Task<bool> RegisterDeviceAsync()
        {
            return Task.FromResult(true);
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
                throw;
            }
        }
    }
}
