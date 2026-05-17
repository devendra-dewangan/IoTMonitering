using IoTMonitoring.App.Repository;
using IoTMonitoring.Data;
using IoTMonitoring.Grpc;
using IoTMonitoring.Hubs;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5050, o =>
    {
        o.Protocols = HttpProtocols.Http1;
    });

    // gRPC
    options.ListenLocalhost(7218, o =>
    {
        o.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddGrpc();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapHub<TelemetryHub>("/hubs/telemetry");
app.MapGrpcService<TelemetryGrpcService>();
app.Run();