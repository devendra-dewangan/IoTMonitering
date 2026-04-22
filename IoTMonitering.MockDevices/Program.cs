using System.Text.Json;
using DeviceMock.Clients;
using DeviceMock.Interface;
using DeviceMock.Models;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: dotnet run -- <mode> <serverAddress> <deviceName>");
            Console.WriteLine("Example: dotnet run -- ws https://localhost:5003/ws \"Office Sensor\"");
            return;
        }

        string mode = args[0];
        string endpoint = args[1];
        string deviceName = args[2];
        string registerUrl = "http://localhost:5000/api/register";
        string approvalUrl = "http://localhost:5000/api/approve";

        var deviceInfo = LoadDeviceInfo();

        if (deviceInfo == null)
        {
            if (deviceInfo == null)
            {
                Console.WriteLine("❌ Registration failed. Exiting.");
                return;
            }
            SaveDeviceInfo(deviceInfo);
        }

        // Check if the device is approved before sending telemetry
        //var approvalClient = new ApprovalClient(approvalUrl);
        //bool isApproved = await approvalClient.CheckApprovalAsync(deviceInfo.DeviceId);

        //if (!isApproved)
        //{
        //    Console.WriteLine("❌ Device not approved. Cannot send telemetry.");
        //    return;
        //}

        // Use Factory to get the right client
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
    }

    static DeviceInfo? LoadDeviceInfo()
    {
        const string filePath = "device_info.json";
        if (!File.Exists(filePath)) return null;
        return JsonSerializer.Deserialize<DeviceInfo>(File.ReadAllText(filePath));
    }

    static void SaveDeviceInfo(DeviceInfo device)
    {
        File.WriteAllText("device_info.json", JsonSerializer.Serialize(device, new JsonSerializerOptions { WriteIndented = true }));
    }
}
