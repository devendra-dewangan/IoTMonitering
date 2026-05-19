using Grpc.Core;
using IoTMonitering.Domain.Entity;
using IoTMonitering.Domain.Protos;
using Microsoft.Extensions.Logging;

namespace DeviceMock.Clients
{
    public class TelemetryGrpcClient : IClient, IDisposable
    {
        private readonly TelemetryService.TelemetryServiceClient _client;
        private readonly ILogger<TelemetryGrpcClient> _logger;
        private AsyncClientStreamingCall<TelemetryRequest, TelemetryResponse>? _call;
        private bool _disposed = false;

        public TelemetryGrpcClient(TelemetryService.TelemetryServiceClient client
                                    , ILogger<TelemetryGrpcClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public Task<bool> ConnectAsync()
        {
            try
            {
                _call = _client.StreamTelemetry();
                _logger.LogInformation("Telemetry stream connected");
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to telemetry stream");
            }

            return Task.FromResult(false);

        }

        public Task<bool> RegisterDeviceAsync()
        {
            return Task.FromResult(true);
        }

        public async Task SendTelemetryAsync(Telemetry telemetry)
        {
            var telemetryRequest = new TelemetryRequest
            {
                DeviceId = telemetry.DeviceId,
                Temperature = telemetry.Temperature,
                Humidity = telemetry.Humidity,
            };

            await _call!.RequestStream.WriteAsync(telemetryRequest);
        }

        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_call == null) return;
                    await _call!.RequestStream.CompleteAsync();
                    var response = await _call!.ResponseAsync;
                    _logger.LogInformation(response.Message);
                    _call.Dispose();
                }
                _disposed = true;
            }
        }


        public void Dispose()
        {
            DisposeAsync(disposing: true).Wait();
        }
    }
}
