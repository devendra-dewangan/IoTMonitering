using DeviceMock.Clients;
using IoTMonitering.Domain.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockDevices.Configurations;

namespace DeviceMock.HostedServices;

public class DeviceWoker : BackgroundService
{
    private readonly DeviceInfo _deviceInfo;
    private readonly ILogger<DeviceWoker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private int retryMax = 20;

    public DeviceWoker(IServiceProvider services
                        , IOptions<DeviceInfo> options
                        , ILogger<DeviceWoker> logger)
    {
        _deviceInfo = options.Value;
        _serviceProvider = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Device worker started.");
        var client = _serviceProvider.GetRequiredKeyedService<IClient>(_deviceInfo.ProtocolType);
        for (int i = 0; i < retryMax && !stoppingToken.IsCancellationRequested; i++)
        {
            if(!await StartService(client, stoppingToken))
            {
                if (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation($"Wainting 10s before retry");
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            }
        } 
        
        _logger.LogInformation("Device worker stopped.");
    }

    private async Task<bool> StartService(IClient client, CancellationToken stoppingToken)
    {
        if (!await client.ConnectAsync())
        {
            _logger.LogError("Failed to connect to telemetry client.");
            return false;
        }

        if (!await client.RegisterDeviceAsync())
        {
            _logger.LogError("Failed to register device.");
            return false;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Sending telemetry data...");
                Telemetry telemetry = new()
                {
                    Timestamp = DateTime.UtcNow,
                    Temperature = new Random().Next(-20, 50),
                    Humidity = new Random().Next(0, 100)
                };
                await client.SendTelemetryAsync(telemetry);
                await Task.Delay(_deviceInfo.DelayMs, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error sending telemetry data.");
                break;
            }

        }

        return true;
    }
}