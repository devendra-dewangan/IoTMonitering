using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using DeviceMock.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviceMock.Clients
{
    internal class TelemetryTcpClient : TelemetryClient
    {
        private readonly string _host;
        private readonly int _port;
        private readonly TcpClient _tcpClient = new TcpClient();

        public TelemetryTcpClient(string endpoint) : base(endpoint)
        {
            var parts = endpoint.Split(':');
            _host = parts[0];
            _port = int.Parse(parts[1]);
        }

        public override bool IsDeviceRegistered(string deviceId)
        {
            return false;
        }

        public override async Task SendTelemetryAsync(Telemetry telemetry)
        {
            try
            {
                await _tcpClient.ConnectAsync(_host,_port);
                var json = JsonSerializer.Serialize(telemetry);
                var bytes = Encoding.UTF8.GetBytes(json);
                using var stream = _tcpClient.GetStream();
                await stream.WriteAsync(bytes, 0, bytes.Length);
                Console.WriteLine($"[TCP] {telemetry.DeviceId} → Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TCP ERROR] {ex.Message}");
            }

        }
    }
}
