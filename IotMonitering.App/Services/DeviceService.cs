using IoTMonitoring.App.Repository;
using IoTMonitoring.Models;
using IoTMonitoring.Models.DTOs;
namespace IoTMonitoring.App.Services
{
    public class DeviceService(IUnitOfWork _unitOfWork) : IDeviceService
    {

        public async Task<Device?> AddDeviceAsync(DeviceCreateDto dto)
        {
            var user = _unitOfWork.Users.GetByIdAsync(dto.userID);
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
            var user = _unitOfWork.Users.GetByIdAsync(userId);
            return _unitOfWork.Devices.GetDevicesByUserIdAsync(user.Id);
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