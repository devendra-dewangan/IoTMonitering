using IoTMonitering.Domain.Entity;

namespace IoTMonitoring.DTOs
{
    public class DeviceCreateDto
    {
        public string userID { get; set; } = string.Empty;
        public string deviceID { get; set; } = string.Empty;
        public string deviceName { get; set; } = string.Empty;
        public DeviceType Type { get; set; } = DeviceType.Unknown; // e.g., Sensor, Actuator
    }

    public class DeviceUpdateDto
    {
        public string userID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DeviceType Type { get; set; } = DeviceType.Unknown;
    }

    public class DeviceReadDto
    {
        public string userID { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DeviceType Type { get; set; } = DeviceType.Unknown;
    }

    public class DeviceAuthReq
    {
        public string deviceID { get; set; }
    }
}
