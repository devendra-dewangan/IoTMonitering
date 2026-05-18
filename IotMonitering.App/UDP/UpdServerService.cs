using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IoTMonitoring.UDP
{
    public class UpdServerService : BackgroundService, IDisposable
    {
        private readonly ILogger<UpdServerService> _logger;
        private readonly UdpClient _udpServer;

        public UpdServerService(ILogger<UpdServerService> logger)
        {
            _logger = logger;
            _udpServer = new UdpClient(6000);
            _logger.LogInformation(
            "UDP Server Started On Port 6000");
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