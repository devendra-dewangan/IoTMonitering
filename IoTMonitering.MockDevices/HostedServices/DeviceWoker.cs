using DeviceMock.Clients;
using DeviceMock.Models;
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

    public DeviceWoker(IServiceProvider services
                        , IOptions<DeviceInfo> options
                        ,ILogger<DeviceWoker> logger)
    {
        _deviceInfo = options.Value;
        _serviceProvider = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Device worker started.");
        var client = _serviceProvider.GetRequiredKeyedService<IClient>(_deviceInfo.ProtocolType);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Sending telemetry data...");
                Telemetry telemetry = new()
                {
                    DeviceId = Guid.NewGuid().ToString(),
                    Timestamp = DateTime.UtcNow,
                    Temperature = new Random().Next(-20, 50),
                    Humidity = new Random().Next(0, 100)
                };
                await client.SendTelemetryAsync(telemetry);
                await Task.Delay(_deviceInfo.DelayMs, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending telemetry data.");
                throw;
            }
            
        }
        _logger.LogInformation("Device worker stopped.");
    }
}