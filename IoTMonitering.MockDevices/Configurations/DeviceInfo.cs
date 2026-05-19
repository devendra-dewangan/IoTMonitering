using DeviceMock.Clients;

namespace MockDevices.Configurations;

public class DeviceInfo
{
    public string DeviceName { get; set; } = string.Empty;
    public string DeviceType { get; set; }  = string.Empty;
    public string Location { get; set; } = string.Empty;
    public ProtocolType ProtocolType { get; set; }
    public int DelayMs { get; set; } = 1000;
}