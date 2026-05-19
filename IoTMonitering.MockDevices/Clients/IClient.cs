using IoTMonitering.Domain.Entity;

namespace DeviceMock.Clients
{
    public interface IClient
    {
        Task<bool> ConnectAsync();
        Task<bool> RegisterDeviceAsync();
        Task SendTelemetryAsync(Telemetry telemetry);
    }

    public enum ProtocolType
    {
        Rest,
        Hub,
        Tcp,
        Udp,
        Grpc
    }
}
