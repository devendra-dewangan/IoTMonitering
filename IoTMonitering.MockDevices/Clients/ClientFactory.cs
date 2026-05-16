using DeviceMock.Interface;

namespace DeviceMock.Clients
{
    public class ClientFactory : IClientFactory
    {
        public IClient GetClient(Protocol type)
        {
            return type switch
            {
                Protocol.Rest => new TelemetryRestClient(),
                Protocol.Hub => new TelemtryHubClient(),
                Protocol.WebSocket => new TelemetryWebsocketClient(),
                Protocol.Tcp => new TelemetryTcpClient(),
                Protocol.Udp => new TelemetryUdpClient(),
                Protocol.Grpc => new TelemetryGrpcClient(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
