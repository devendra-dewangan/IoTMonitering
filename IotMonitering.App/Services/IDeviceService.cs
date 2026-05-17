using IoTMonitering.Domain.Entity;
using IoTMonitoring.DTOs;

namespace IoTMonitoring.App.Services
{
    public interface IDeviceService
    {
        Task<IEnumerable<Device>> GetAllDevicesAsync(string userId);
        Task<Device?> GetDeviceByIdAsync(string id);
        Task<Device?> AddDeviceAsync(DeviceCreateDto device);
        Task<Device?> UpdateDeviceAsync(string id, DeviceUpdateDto dto);
        Task DeleteDeviceAsync(string id);
    }
}