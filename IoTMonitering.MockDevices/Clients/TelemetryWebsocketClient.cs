using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DeviceMock.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviceMock.Clients
{
    internal class TelemetryWebsocketClient : TelemetryClient
    {
        private readonly ClientWebSocket clientWebSocket = new ClientWebSocket();
        public TelemetryWebsocketClient(string endpoint) : base(endpoint)
        {
            clientWebSocket.ConnectAsync(new Uri(endpoint), CancellationToken.None).RunSynchronously();
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
