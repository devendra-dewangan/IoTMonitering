using DeviceMock.Models;
using Microsoft.AspNetCore.SignalR.Client;


namespace DeviceMock.Clients
{
    internal class TelemtryHubClient : TelemetryClient
    {
        private readonly HubConnection hubConnection;
        public TelemtryHubClient(string endpoint) : base(endpoint)
        {
            hubConnection =new HubConnectionBuilder()
                .WithUrl(endpoint)
                .WithAutomaticReconnect()
                .Build();
            hubConnection.StartAsync().RunSynchronously();
            Console.WriteLine("[SignalR] Connected");
        }

        public override bool IsDeviceRegistered(string deviceId)
        {
            return false;
        }

        public override async Task SendTelemetryAsync(Telemetry data)
        {
            try
            {
                await hubConnection.InvokeAsync("SendTelemetry", data);
                Console.WriteLine($"[SignalR] {data.DeviceId} → Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SignalR ERROR] {ex.Message}");
            }
        }
    }
}
