using DeviceMock.Models;

namespace DeviceMock.Clients
{
    public interface IClient
    {
        bool IsDeviceRegistered(string deviceId);

        Task SendTelemetryAsync(Telemetry telemetry);
    }

    public enum ProtocolType
    {
        Rest,
        Hub,
        WebSocket,
        Tcp,
        Udp,
        Grpc
    }
}
