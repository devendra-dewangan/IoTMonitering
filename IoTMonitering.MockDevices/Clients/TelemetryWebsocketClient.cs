using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using DeviceMock.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockDevices.Configurations;

namespace DeviceMock.Clients
{
    internal class TelemetryWebsocketClient : IClient
    {
        private readonly ClientWebSocket _clientWebSocket;
        private readonly ILogger _logger;

        public TelemetryWebsocketClient(ILogger<TelemetryWebsocketClient> logger,IOptions<ServerInfo> serverInfo)
        {
            _logger = logger;
            _clientWebSocket = new ClientWebSocket();
            _clientWebSocket.ConnectAsync(new Uri(serverInfo.Value.ServerUri), CancellationToken.None).RunSynchronously();
            _logger.LogInformation("[WebSocket] Connected");
        }

        public bool IsDeviceRegistered(string deviceId)
        {
            return false;
        }

        public async Task SendTelemetryAsync(Telemetry data)
        {
            var json = JsonSerializer.Serialize(data);
            var bytes = Encoding.UTF8.GetBytes(json);
            await _clientWebSocket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
            _logger.LogInformation($"[WebSocket] {data.DeviceId} → Sent");
        }
    }
}
