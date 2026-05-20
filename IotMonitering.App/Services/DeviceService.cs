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
        Task<Device?> AddDeviceAsync(DeviceCreateDto device);
        Task<Device?> UpdateDeviceAsync(string id, DeviceUpdateDto dto);
        Task DeleteDeviceAsync(string id);
    }
    public class DeviceService(IUnitOfWork _unitOfWork, UserManager<User> _userManager) : IDeviceService
    {

        public async Task<Device?> AddDeviceAsync(DeviceCreateDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.userID);
            var key = Guid.NewGuid().ToString();
            var device = new Device
            {
                Name = dto.Name,
                DeviceKey = key,
                UserId = user.Id,
                Type = dto.Type,
            };

            await _unitOfWork.Devices.AddAsync(device);
            return await _unitOfWork.CommitAsync() > 0 ? device : null;
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
    }
}