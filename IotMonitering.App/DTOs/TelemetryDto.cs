namespace IoTMonitoring.DTOs;

public class TelemetryCreateDto
{
    public double Temperature { get; set; }
    public double Humidity { get; set; }
}

public class TelemetryReadDto
{
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public DateTime Timestamp { get; set; }
}
