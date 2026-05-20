using IoTMonitering.Domain.Entity;
using IoTMonitoring.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockDevices.Configurations;
using System.Text;
using System.Text.Json;

namespace DeviceMock.Clients
{
    internal class TelemetryRestClient : IClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TelemetryRestClient> _logger;
        private readonly ServerInfo _serverInfo;
        private readonly DeviceInfo _deviceInfo;

        public TelemetryRestClient(HttpClient httpClient, ILogger<TelemetryRestClient> logger
            , IOptions<ServerInfo> options,IOptions<DeviceInfo> deviceOptions)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serverInfo = options.Value;
            _deviceInfo = deviceOptions.Value;
        }

        public Task<bool> ConnectAsync()
        {
            return Task.FromResult(true);
        }

        public async Task<bool> RegisterDeviceAsync()
        {
            var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, _serverInfo.RegisterPath)
            {
                Content = new StringContent(JsonSerializer.Serialize(new DeviceCreateDto
                {
                    deviceID = _deviceInfo.DeviceId,
                    deviceName = _deviceInfo.DeviceName
                }), Encoding.UTF8, "application/json")
            });
            _logger.LogInformation($"[REST] {_deviceInfo.DeviceId} → {response.StatusCode}");
            return true;
        }

        public async Task SendTelemetryAsync(Telemetry data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, _serverInfo.TelemetryPath) { Content = content });
                _logger.LogInformation($"[REST] {data.DeviceId} → {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[REST ERROR] {ex.Message}");
                throw;
            }
        }
    }
}
