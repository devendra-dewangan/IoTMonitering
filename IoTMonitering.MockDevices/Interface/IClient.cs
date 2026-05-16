using DeviceMock.Models;

namespace DeviceMock.Interface
{
    public interface IClient
    {
        bool IsDeviceRegistered(string deviceId);

        Task SendTelemetryAsync(Telemetry telemetry);
    }
}
