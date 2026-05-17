using Grpc.Core;
using IoTMonitering.Domain.Protos;
using IoTMonitering.Domain.Entity;

namespace IoTMonitoring.Grpc
{
    public class TelemetryGrpcService : TelemetryService.TelemetryServiceBase
    {
        private readonly ILogger _logger;

        public TelemetryGrpcService(ILogger<TelemetryGrpcService> logger)
        {
            _logger = logger;
        }

        public override async Task<TelemetryResponse> StreamTelemetry(
            IAsyncStreamReader<TelemetryRequest> requestStream,
            ServerCallContext context)
        {
            await foreach (var telemetryReq in requestStream.ReadAllAsync())
            {

                var telemetry = new Telemetry
                {
                    DeviceId = telemetryReq.DeviceId,
                    Temperature = telemetryReq.Temperature,
                    Humidity = telemetryReq.Humidity,
                    Timestamp = DateTime.UtcNow
                };

                _logger.LogInformation("Received telemetry from Device {DeviceId}: Temp={Temperature}, Humidity={Humidity}",
                    telemetryReq.DeviceId, telemetryReq.Temperature, telemetryReq.Humidity);

                // Optional: broadcast to SignalR hub if you want live updates
            }

            return new TelemetryResponse { Message = "Stream ended" };
        }
    }
}
