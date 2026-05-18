using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using IoTMonitering.Domain.Entity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockDevices.Configurations;

namespace DeviceMock.Clients
{
    internal class TelemetryTcpClient : IClient, IDisposable
    {
        private readonly TcpClient _tcpClient;
        private readonly ILogger<TelemetryTcpClient> _logger;
        private readonly NetworkStream _stream;
        private bool _disposed = false;

        public TelemetryTcpClient(ILogger<TelemetryTcpClient> logger,
                                 IOptions<ServerInfo> serverInfo)
        {
            _tcpClient = new TcpClient(
                serverInfo.Value.ServerUri, serverInfo.Value.ServerPort);
            _logger = logger;
            _stream = _tcpClient.GetStream();
            _logger.LogInformation($"[TCP] Initialized for {serverInfo.Value.ServerUri}:{serverInfo.Value.ServerPort}");
        }

        public void Dispose()
        {
            if (_disposed) return;

            _stream.Close();
            _stream.Dispose();
            _tcpClient.Close();
            _tcpClient.Dispose();
            _disposed = true;
        }

        public bool IsDeviceRegistered(string deviceId)
        {
            return false;
        }

        public async Task SendTelemetryAsync(Telemetry telemetry)
        {
            try
            {
                var json = JsonSerializer.Serialize(telemetry);
                var bytes = Encoding.UTF8.GetBytes(json);
                await _stream.WriteAsync(bytes, 0, bytes.Length);
                _logger.LogInformation($"[TCP] {telemetry.DeviceId} → Sent");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[TCP ERROR] {ex.Message}");
            }

        }

    }
}
