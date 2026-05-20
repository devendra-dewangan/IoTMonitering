using IoTMonitering.Domain.Entity;
using IoTMonitoring.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitoring.App.Repository
{
    public class UnitOfWork(AppDbContext _context) : IUnitOfWork
    {
        private readonly Lazy<IDeviceRepository> _deviceRepository = new(() => new DeviceRepository(_context));

        public IDeviceRepository Devices => _deviceRepository.Value;

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}