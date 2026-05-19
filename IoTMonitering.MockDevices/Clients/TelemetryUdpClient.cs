using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using IoTMonitering.Domain.Entity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockDevices.Configurations;

namespace DeviceMock.Clients
{
    public class TelemetryUdpClient : IClient, IDisposable
    {
        
        private readonly UdpClient _udpClient;
        private readonly ServerInfo _serverInfo;
        private readonly ILogger _logger;
        private bool _isDisposed = false;

        public TelemetryUdpClient(ILogger<TelemetryUdpClient> logger,
            IOptions<ServerInfo> serverInfo) 
        { 
            _logger = logger; 
            _udpClient = new UdpClient();
            _serverInfo = serverInfo.Value;
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                _udpClient.Connect(_serverInfo.ServerUri, _serverInfo.ServerPort);
                return true;
               
            }
            catch (Exception ex)
            {
                CleanUp();
                await Task.Delay(5000);
                _logger.LogError($"Unable to connect {ex.Message}");
            }
            return false;
          
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            CleanUp();
            _isDisposed = true;
        }

        private void CleanUp()
        {
            _udpClient.Close();
            _udpClient.Dispose();
        }

        public Task<bool> RegisterDeviceAsync()
        {
            return Task.FromResult(true);
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
                throw;
            }
        }
    }
}
