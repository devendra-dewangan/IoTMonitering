using DeviceMock.Interface;

namespace DeviceMock.Clients
{
    public interface IClientFactory
    {
        IClient GetClient(Protocol type);
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