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
        private readonly ServerInfo _serverInfo;
        private readonly ILogger<TelemetryTcpClient> _logger;
        private NetworkStream? _stream;
        private TcpClient? _tcpClient;
        private bool _disposed = false;

        public TelemetryTcpClient(ILogger<TelemetryTcpClient> logger,
                                 IOptions<ServerInfo> serverInfo)
        {

            _serverInfo = serverInfo.Value;
            _logger = logger;
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(_serverInfo.ServerUri, _serverInfo.ServerPort);
                _logger.LogInformation($"[TCP] Initialized for {_serverInfo.ServerUri}:{_serverInfo.ServerPort}");
                _stream = _tcpClient.GetStream();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to connect {ex.Message}");
                CleanUp();
                _logger.LogInformation("Waiting 5s For TCP Client clean up");
                await Task.Delay(5000);
            }
            return false;
        }

        public Task<bool> RegisterDeviceAsync()
        {
            return Task.FromResult(true);
        }

        public async Task SendTelemetryAsync(Telemetry telemetry)
        {
            try
            {
                var json = JsonSerializer.Serialize(telemetry);
                var bytes = Encoding.UTF8.GetBytes(json);
                await _stream!.WriteAsync(bytes);
                _logger.LogInformation($"[TCP] {telemetry.DeviceId} → Sent");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[TCP ERROR] {ex.Message}");
                throw;
            }

        }

        public void Dispose()
        {
            if (_disposed) return;
            CleanUp();
            _disposed = true;
        }

        private void CleanUp()
        {
            _stream?.Close();
            _stream?.Dispose();
            _tcpClient?.Close();
            _tcpClient?.Dispose();
        }
    }
}
