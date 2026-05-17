using IoTMonitering.Domain.Entity;
using IoTMonitoring.Data;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitoring.App.Repository
{
    public interface IDeviceRepository : IRepository<Device>
    {
        Task<IEnumerable<Device>> GetDevicesByUserIdAsync(int userId);   
    }
    public class DeviceRepository(AppDbContext _context) : IDeviceRepository
    {
        public async Task AddAsync(Device entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            _context.Remove(new Device() { DeviceKey = id });
        }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            return [.. _context.Devices];
        }

        public async Task<Device?> GetByIdAsync(string id)
        {
            return await _context.Devices.FirstOrDefaultAsync(d => d.DeviceKey == id);
        }

        public async Task<IEnumerable<Device>> GetDevicesByUserIdAsync(int userId)
        {
            return [.. _context.Devices.Where(x => x.UserId == userId)];
        }

        public async Task UpdateAsync(Device entity)
        {
            _context.Update(entity);
        }
    }
}