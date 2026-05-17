using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using DeviceMock.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockDevices.Configurations;

namespace DeviceMock.Clients
{
    internal class TelemetryUdpClient : IClient
    {
        
        private readonly UdpClient _udpClient = new UdpClient();
        private readonly ILogger _logger;

        public TelemetryUdpClient(ILogger<TelemetryUdpClient> logger,
            IOptions<ServerInfo> serverInfo) 
        { 
            _logger = logger; 
            _udpClient = new UdpClient(serverInfo.Value.ServerUri, serverInfo.Value.ServerPort);
        }
       

        public bool IsDeviceRegistered(string deviceId)
        {
            return false;
        }

        public async Task SendTelemetryAsync(Telemetry data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var bytes = Encoding.UTF8.GetBytes(json);
                await _udpClient.SendAsync(bytes, bytes.Length);
                _logger.LogInformation($"[UDP] {data.DeviceId} → Sent");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UDP ERROR] {ex.Message}");
            }
        }
    }
}
