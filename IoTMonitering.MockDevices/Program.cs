using DeviceMock.Clients;
using DeviceMock.HostedServices;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MockDevices.Configurations;
using IoTMonitering.Domain.Protos;

var builder = Host.CreateApplicationBuilder(args);

AppContext.SetSwitch(
    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport",
    true);

builder.Services.Configure<DeviceInfo>(
    builder.Configuration.GetSection(nameof(DeviceInfo)));

builder.Services.Configure<ServerInfo>(
    builder.Configuration.GetSection(nameof(ServerInfo)));

builder.Services.AddHostedService<DeviceWoker>();

builder.Services.AddHttpClient<TelemetryRestClient>(
    (provider,client) =>
    {
        var serverInfo = provider
            .GetRequiredService<IOptions<ServerInfo>>()
            .Value;
        client.BaseAddress = new Uri(serverInfo.ServerUri);
    });
builder.Services.AddKeyedScoped<IClient, TelemetryRestClient>(
    ProtocolType.Rest
    ,(provider,_) => provider.GetRequiredService<TelemetryRestClient>());

builder.Services.AddSingleton(provider =>
{
    var otions = provider.GetRequiredService<IOptions<ServerInfo>>().Value;
    return new HubConnectionBuilder()
        .WithUrl(otions.ServerUri)
        .Build();
});
builder.Services.AddKeyedScoped<IClient, TelemtryHubClient>(ProtocolType.Hub);

builder.Services.AddKeyedScoped<IClient, TelemetryTcpClient>(ProtocolType.Tcp);
builder.Services.AddKeyedScoped<IClient, TelemetryUdpClient>(ProtocolType.Udp);
builder.Services.AddKeyedScoped<IClient, TelemetryWebsocketClient>(ProtocolType.WebSocket);

builder.Services.AddGrpcClient<TelemetryService.TelemetryServiceClient>((provider, client) =>
{
    var serverInfo = provider.GetRequiredService<IOptions<ServerInfo>>().Value;
    client.Address = new Uri(serverInfo.ServerUri);
});
builder.Services.AddKeyedScoped<IClient, TelemetryGrpcClient>(ProtocolType.Grpc);

var app = builder.Build();
await app.RunAsync();