using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using DeviceMock.Models;

namespace DeviceMock.Clients
{
    internal class TelemetryWebsocketClient : TelemetryClient
    {
        private readonly ClientWebSocket clientWebSocket = new ClientWebSocket();
        public TelemetryWebsocketClient()
        {
            clientWebSocket.ConnectAsync(new Uri("endpoint"), CancellationToken.None).RunSynchronously();
            Console.WriteLine("[WebSocket] Connected");
        }

        public override bool IsDeviceRegistered(string deviceId)
        {
            return false;
        }

        public override async Task SendTelemetryAsync(Telemetry data)
        {
            var json = JsonSerializer.Serialize(data);
            var bytes = Encoding.UTF8.GetBytes(json);
            await clientWebSocket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine($"[WebSocket] {data.DeviceId} → Sent");
        }
    }
}
