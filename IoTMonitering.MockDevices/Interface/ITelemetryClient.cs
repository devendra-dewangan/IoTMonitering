using DeviceMock.Models;

namespace DeviceMock.Interface
{
    internal interface ITelemetryClient
    {
        bool IsDeviceRegistered(string deviceId);

        Task SendTelemetryAsync(Telemetry telemetry);
    }
}
