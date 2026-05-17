namespace IoTMonitering.Domain.Entity
{
    public enum DeviceType
    {
        Unknown = 0,
        Sensor,
        Actuator
    }
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DeviceKey { get; set; } = string.Empty;
        public DeviceType Type { get; set; } = DeviceType.Unknown;
        public bool IsActive { get; set; } = true;
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
