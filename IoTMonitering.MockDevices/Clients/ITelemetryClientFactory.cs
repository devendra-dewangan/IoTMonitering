using DeviceMock.Interface;

namespace DeviceMock.Clients
{
    public interface ITelemetryClientFactory
    {
        ITelemetryClient GetClient(Protocol type);
    }

    public enum Protocol
    {
        Rest,
        Hub,
        WebSocket,
        Tcp,
        Udp,
        Grpc
    }
}