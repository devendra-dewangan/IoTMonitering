using DeviceMock.Clients;
using DeviceMock.HostedServices;
using DeviceMock.Interface;
using DeviceMock.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockDevices.Configurations;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<DeviceInfo>(
    builder.Configuration.GetSection(nameof(DeviceInfo)));

builder.Services.Configure<ServerInfo>(
    builder.Configuration.GetSection(nameof(ServerInfo)));

builder.Services.AddScoped<ITelemetryClientFactory, TelemetryClientFactory>();
builder.Services.AddHostedService<DeviceWoker>();

ITelemetryClient? telemetryClient = TelemetryClientFactory.Create(mode, endpoint);
if (telemetryClient == null) return;


Console.WriteLine($"📡 Sending telemetry from '{deviceInfo.DeviceName}' using {mode.ToUpper()}...");

var rand = new Random();

while (true)
{
    var telemetry = new Telemetry
    {
        DeviceId = deviceInfo.DeviceId,
        DeviceName = deviceInfo.DeviceName,
        Temperature = 20 + rand.NextDouble() * 10,
        Humidity = 40 + rand.NextDouble() * 20,
        Timestamp = DateTime.UtcNow
    };

    await telemetryClient.SendTelemetryAsync(telemetry);
    await Task.Delay(1000);
}
