using DeviceMock.Clients;
using DeviceMock.Interface;
using DeviceMock.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockDevices.Configurations;

namespace DeviceMock.HostedServices;

public class DeviceWoker : BackgroundService
{
    private readonly ITelemetryClient _telemetryClient;
    private readonly DeviceInfo _options;
    private readonly ILogger<DeviceWoker> _logger;

    public DeviceWoker(ITelemetryClientFactory telemetryClientFactory
                        , IOptions<DeviceInfo> options
                        ,ILogger<DeviceWoker> logger)
    {
        _options = options.Value;
        _telemetryClient = telemetryClientFactory.GetClient(_options.Protocol);
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Device worker started.");
        while (stoppingToken.IsCancellationRequested)
        {
            Telemetry telemetry = new()
            {
                DeviceId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow,
                Temperature = new Random().Next(-20, 50),
                Humidity = new Random().Next(0, 100)
            };
            await _telemetryClient.SendTelemetryAsync(telemetry);
            await Task.Delay(_options.DelayMs, stoppingToken);
        }
        _logger.LogInformation("Device worker stopped.");
    }
}