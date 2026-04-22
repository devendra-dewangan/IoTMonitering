using IoTMonitoring.Data;
using IoTMonitoring.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitoring.App.Repository
{
    public class DeviceRepository(AppDbContext _context) : IRepository<Device>
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

        public async Task UpdateAsync(Device entity)
        {
            _context.Update(entity);
        }
    }
}