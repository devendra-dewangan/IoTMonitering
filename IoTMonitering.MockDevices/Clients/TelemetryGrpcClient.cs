using DeviceMock.Models;

namespace DeviceMock.Clients
{
    internal class TelemetryGrpcClient : IClient
    {
        public TelemetryGrpcClient()
        {
        }

        public bool IsDeviceRegistered(string deviceId)
        {
            throw new NotImplementedException();
        }

        public Task SendTelemetryAsync(Telemetry telemetry)
        {
            throw new NotImplementedException();
        }
    }
}
