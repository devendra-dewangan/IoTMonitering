using IoTMonitoring.Data;
using IoTMonitoring.Models;

namespace IoTMonitoring.App.Repository
{
    public class UnitOfWork(AppDbContext _context) : IUnitOfWork
    {
        private IRepository<User>? _userRepository;
        private IDeviceRepository _deviceRepository;
        
        public IRepository<User> Users => _userRepository ??= new UserRepository(_context);
        public IDeviceRepository Devices => _deviceRepository ??= new DeviceRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}