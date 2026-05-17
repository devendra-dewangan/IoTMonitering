namespace IoTMonitering.Domain.Entity
{
    public class Telemetry
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public Device? Device { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public DateTime Timestamp {  get; set; } = DateTime.UtcNow;
    }
}
