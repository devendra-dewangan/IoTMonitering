using DeviceMock.Clients;
using DeviceMock.HostedServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockDevices.Configurations;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<DeviceInfo>(
    builder.Configuration.GetSection(nameof(DeviceInfo)));

builder.Services.Configure<ServerInfo>(
    builder.Configuration.GetSection(nameof(ServerInfo)));

builder.Services.AddHostedService<DeviceWoker>();
builder.Services.AddKeyedScoped<IClient, TelemetryRestClient>(ProtocolType.Rest);

var app = builder.Build();
app.Run();



