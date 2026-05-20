using IoTMonitering.Domain.Entity;
using IoTMonitoring.App.Repository;
using IoTMonitoring.DTOs;
using Microsoft.AspNetCore.Identity;
namespace IoTMonitoring.App.Services
{
    public interface IDeviceService
    {
        Task<IEnumerable<Device>> GetAllDevicesAsync(string userId);
        Task<Device?> GetDeviceByIdAsync(string id);
        Task<Device?> RegisterDevice(string key, string userId);
        Task<Device?> UpdateDeviceAsync(string id, DeviceUpdateDto dto);
        Task DeleteDeviceAsync(string id);
        Task AddDeviceToTempList(DeviceCreateDto device);
        IEnumerable<Device> GetRequestedDevices(string userId);
    }
    public class DeviceService(IUnitOfWork _unitOfWork, UserManager<User> _userManager) : IDeviceService
    {
        private static Dictionary<string, List<Device>> _tempDeivceList = new();

        public async Task<Device?> RegisterDevice(string key, string userId)
        {
            if (_tempDeivceList.TryGetValue(userId, out var devices))
            {
                var device = devices.FirstOrDefault(d => d.DeviceKey == key);

                if (device != null)
                {
                    devices.Remove(device);
                    _tempDeivceList[userId] = devices;
                    await _unitOfWork.Devices.AddAsync(device);
                    return await _unitOfWork.CommitAsync() > 0 ? device : null;
                }
            }
            return null;
        }

        public Task DeleteDeviceAsync(string id)
        {
            return _unitOfWork.Devices.DeleteAsync(id);
        }

        public Task<IEnumerable<Device>> GetAllDevicesAsync(string userId)
        {
            return _unitOfWork.Devices.GetDevicesByUserIdAsync(userId);
        }

        public Task<Device?> GetDeviceByIdAsync(string id)
        {
            return _unitOfWork.Devices.GetByIdAsync(id);
        }

        public Task<Device?> UpdateDeviceAsync(string id, DeviceUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task AddDeviceToTempList(DeviceCreateDto device)
        {
            _tempDeivceList.TryGetValue(device.userID, out var devices);
            devices ??= new List<Device>();
            if (!devices.Exists(x => x.DeviceKey == device.deviceID))
            {
                devices.Add(new Device
                {
                    DeviceKey = device.deviceID,
                    Type = device.Type,
                });
            }
            _tempDeivceList[device.userID] = devices;
            return Task.CompletedTask;
        }

        public IEnumerable<Device> GetRequestedDevices(string userId)
        {
            if (_tempDeivceList.TryGetValue(userId, out var devices))
            {
                return devices;
            }
            return [];
        }

    }
}