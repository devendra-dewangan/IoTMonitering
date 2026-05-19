using IoTMonitoring.Config;
using Microsoft.Extensions.Options;
using System.Net.Sockets;
using System.Text;

namespace IoTMonitoring.UDP
{
    public class UdpServerService : BackgroundService, IDisposable
    {
        private readonly ILogger<UdpServerService> _logger;
        private readonly UdpClient _udpServer;

        public UdpServerService(ILogger<UdpServerService> logger, IOptions<ServerConfiguration> options)
        {
            _logger = logger;
            _udpServer = new UdpClient(options.Value.Udp.Port);
            _logger.LogInformation(
            "UDP Server Started On Port {endpoint}", options.Value.Udp.Port);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    UdpReceiveResult result =
                        await _udpServer.ReceiveAsync(
                            stoppingToken);

                    string message =
                        Encoding.UTF8.GetString(
                            result.Buffer);

                    _logger.LogInformation(
                        "Received From {endpoint}: {message}",
                        result.RemoteEndPoint,
                        message);

                    string response =
                        $"ACK: {message}";

                    byte[] responseBytes =
                        Encoding.UTF8.GetBytes(response);

                    await _udpServer.SendAsync(
                        responseBytes,
                        result.RemoteEndPoint,
                        stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation(
                    "UDP Server Stopped");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "UDP Server Error");
            }
        }


        public override void Dispose()
        {
            _udpServer.Close();
            base.Dispose();
        }
    }
}