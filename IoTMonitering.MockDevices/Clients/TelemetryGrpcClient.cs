using IoTMonitering.Domain.Entity;
using IoTMonitering.Domain.Protos;
using Microsoft.Extensions.Logging;

namespace DeviceMock.Clients
{
    internal class TelemetryGrpcClient : IClient
    {
        private readonly TelemetryService.TelemetryServiceClient _client;
        private readonly ILogger<TelemetryGrpcClient> _logger;
private readonly ILogger<TelemetryGrpcClient> _log;
        public TelemetryGrpcClient(TelemetryService.TelemetryServiceClient client
                                    ,ILogger<TelemetryGrpcClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public bool IsDeviceRegistered(string deviceId)
        {
            throw new NotImplementedException();
        }

        public async Task SendTelemetryAsync(Telemetry telemetry)
        {
            using var call = _client.StreamTelemetry();
            var telemetryRequest = new TelemetryRequest
            {
                DeviceId = telemetry.DeviceId,
                Temperature =  telemetry.Temperature,
                Humidity = telemetry.Humidity,
            };



            await call.RequestStream.WriteAsync(telemetryRequest);
                    

            await call.RequestStream.CompleteAsync();

            var response = await call.ResponseAsync;

            _logger.LogInformation(response.Message);
        }
    }
}
