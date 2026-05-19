using IoTMonitering.Domain.Entity;
using IoTMonitoring.Data;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitoring.App.Repository
{
    public class UnitOfWork(AppDbContext _context) : IUnitOfWork
    {
        private readonly Lazy<IRepository<User>> _userRepository = new(() => new UserRepository(_context));
        private readonly Lazy<IDeviceRepository> _deviceRepository = new(() => new DeviceRepository(_context));

        public IRepository<User> Users => _userRepository.Value;
        public IDeviceRepository Devices => _deviceRepository.Value;

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}