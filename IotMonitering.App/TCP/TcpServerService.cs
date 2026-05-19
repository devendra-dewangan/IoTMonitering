using IoTMonitoring.Config;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IoTMonitoring.TCP;

public class TcpServerService : BackgroundService, IDisposable
{
    private readonly ILogger<TcpServerService> _logger;
    private readonly TcpListener _TcpListner;

    public TcpServerService(ILogger<TcpServerService> logger, IOptions<ServerConfiguration> options)
    {
        _logger = logger;
        _TcpListner = new TcpListener(IPAddress.Any, options.Value.Tcp.Port);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _TcpListner.Start();
        _logger.LogInformation(
            "TCP Server Started On {endpoint}", _TcpListner.LocalEndpoint);
        while (!stoppingToken.IsCancellationRequested)
        {

            var client = await _TcpListner.AcceptTcpClientAsync(stoppingToken);

            _ = HandleClient(client, stoppingToken);
        }
    }

    private async Task HandleClient(TcpClient client, CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Client connected: {ClientEndpoint}", client.Client.RemoteEndPoint);

            using NetworkStream stream =
                client.GetStream();

            byte[] buffer = new byte[1024];

            while (!stoppingToken.IsCancellationRequested)
            {
                int bytesRead =
                    await stream.ReadAsync(
                        buffer,
                        stoppingToken);

                if (bytesRead == 0)
                {
                    _logger.LogInformation(
                        "Client disconnected gracefully");

                    break;
                }

                string message =
                    Encoding.UTF8.GetString(
                        buffer,
                        0,
                        bytesRead);

                _logger.LogInformation(
                    "Received: {message}",
                    message);

                string response =
                    $"ACK: {message}";

                byte[] responseBytes =
                    Encoding.UTF8.GetBytes(response);

                await stream.WriteAsync(
                    responseBytes,
                    stoppingToken);
            }
        }
        catch (SocketException ex)
        {
            _logger.LogWarning(
                "Socket disconnected: {message}",
                ex.Message);
        }
        catch (IOException ex)
        {
            _logger.LogWarning(
                "Connection reset: {message}",
                ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected TCP Error");
        }
        finally
        {
            client.Close();

            _logger.LogInformation(
                "Client Disconnected");
        }
    }

    public override void Dispose()
    {
        _TcpListner.Stop();
        base.Dispose();
    }
}