using System.Text;
using System.Text.Json;
using DeviceMock.Models;
using Microsoft.Extensions.Logging;

namespace DeviceMock.Clients
{
    internal class TelemetryRestClient : TelemetryClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public TelemetryRestClient() { }

        public override bool IsDeviceRegistered(string deviceId)
        {
            return false;
        }

        public override async Task SendTelemetryAsync(Telemetry data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_endpoint, content);
                Console.WriteLine($"[REST] {data.DeviceId} → {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[REST ERROR] {ex.Message}");
            }
        }
    }
}
