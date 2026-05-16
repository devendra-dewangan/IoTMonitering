using DeviceMock.Interface;
using DeviceMock.Models;

namespace DeviceMock.Clients
{
    public abstract class TelemetryClient : ITelemetryClient
    {
        protected readonly string _endpoint;

        public abstract bool IsDeviceRegistered(string deviceId);

        public abstract Task SendTelemetryAsync(Telemetry telemetry);
    }
}
