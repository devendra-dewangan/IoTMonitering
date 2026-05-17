using Grpc.Core;
using IoTMonitoring.Data;
using IoTMonitering.Domain.Protos;
using IoTMonitering.Domain.Entity;

namespace IoTMonitoring.Grpc
{
    public class TelemetryGrpcService : TelemetryService.TelemetryServiceBase
    {
        private readonly AppDbContext _context;

        public TelemetryGrpcService(AppDbContext context)
        {
            _context = context;
        }

        public override async Task<TelemetryResponse> StreamTelemetry(
            IAsyncStreamReader<TelemetryRequest> requestStream,
            ServerCallContext context)
        {
            await foreach (var telemetryReq in requestStream.ReadAllAsync())
            {
                var device = await _context.Devices.FindAsync(telemetryReq.DeviceId);
                if (device == null) continue; // skip invalid device

                var telemetry = new Telemetry
                {
                    DeviceId = telemetryReq.DeviceId,
                    Temperature = telemetryReq.Temperature,
                    Humidity = telemetryReq.Humidity,
                    Timestamp = DateTime.UtcNow
                };

                _context.Telemetries.Add(telemetry);
                await _context.SaveChangesAsync();

                // Optional: broadcast to SignalR hub if you want live updates
            }

            return new TelemetryResponse { Message = "Stream ended" };
        }
    }
}
