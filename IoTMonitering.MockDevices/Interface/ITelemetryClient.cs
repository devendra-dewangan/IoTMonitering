using DeviceMock.Models;

namespace DeviceMock.Interface
{
    public interface ITelemetryClient
    {
        bool IsDeviceRegistered(string deviceId);

        Task SendTelemetryAsync(Telemetry telemetry);
    }
}
