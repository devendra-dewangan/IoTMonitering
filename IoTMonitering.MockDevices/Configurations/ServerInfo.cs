namespace MockDevices.Configurations;

public class ServerInfo
{
    public string ServerUri { get; set; }
    public int ServerPort { get; set; }
    public string RegisterPath { get; set; }
    public string AuthPath { get; set; }
    public string TelemetryPath { get; set; }
}