using DeviceMock.Clients;

namespace MockDevices.Configurations;

public class DeviceInfo
{
    public string DeviceName { get; set; } 
    public string DeviceType { get; set; }
    public string Location { get; set; }
    public ProtocolType ProtocolType { get; set; }
    public int DelayMs { get; set; } = 1000;
}