using System.Text;
using System.Text.Json;
using IoTMonitering.Domain.Entity;
using Microsoft.Extensions.Logging;

namespace DeviceMock.Clients
{
    internal class TelemetryRestClient : IClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TelemetryRestClient> _logger;

        public TelemetryRestClient(HttpClient httpClient, ILogger<TelemetryRestClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
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
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/telemetry") { Content = content });
                _logger.LogInformation($"[REST] {data.DeviceId} → {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[REST ERROR] {ex.Message}");
            }
        }
    }
}
