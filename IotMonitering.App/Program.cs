using IoTMonitoring.App.Config;
using IoTMonitoring.App.Repository;
using IoTMonitoring.App.Services;
using IoTMonitoring.Config;
using IoTMonitoring.Data;
using IoTMonitoring.Grpc;
using IoTMonitoring.Hubs;
using IoTMonitoring.TCP;
using IoTMonitoring.UDP;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ServerConfiguration>(builder.Configuration.GetSection(nameof(ServerConfiguration)));
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection(nameof(JwtConfiguration)));

var config = builder.Configuration.GetSection(nameof(ServerConfiguration)).Get<ServerConfiguration>()!;
var jwtConfig = builder.Configuration.GetSection(nameof(JwtConfiguration)).Get<JwtConfiguration>()!;
builder.WebHost.ConfigureKestrel((sp,options) =>
{
    options.ListenAnyIP(config.RestApi.Port, o =>
    {
        o.Protocols = HttpProtocols.Http1;
    });

    options.ListenAnyIP(config.SignalR.Port, o =>
    {
        o.Protocols = HttpProtocols.Http1;
    });

    options.ListenAnyIP(config.Grpc.Port, o =>
    {
        o.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddGrpc();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHostedService<TcpServerService>();
builder.Services.AddHostedService<UdpServerService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtConfig.Key))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<TelemetryHub>(config.SignalR.Route);
app.MapGrpcService<TelemetryGrpcService>();
app.Run();