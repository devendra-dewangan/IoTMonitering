using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using DeviceMock.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviceMock.Clients
{
    internal class TelemetryUdpClient : TelemetryClient
    {
        private readonly string _host;
        private readonly int _port;
        private readonly UdpClient _udpClient = new UdpClient();
        public TelemetryUdpClient(string endpoint) : base(endpoint)
        {
            var parts = endpoint.Split(':');
            _host = parts[0];
            _port = int.Parse(parts[1]);
        }

        public override bool IsDeviceRegistered(string deviceId)
        {
            return false;
        }

        public override async Task SendTelemetryAsync(Telemetry data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var bytes = Encoding.UTF8.GetBytes(json);
                await _udpClient.SendAsync(bytes, bytes.Length,_host,_port);
                Console.WriteLine($"[UDP] {data.DeviceId} → Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UDP ERROR] {ex.Message}");
            }
        }
    }
}
