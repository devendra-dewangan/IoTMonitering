using IoTMonitoring.Data;
using IoTMonitoring.Models;

namespace IoTMonitoring.App.Repository
{
    public class UnitOfWork(AppDbContext _context) : IUnitOfWork
    {
        private IRepository<User>? _userRepository;
        private IRepository<Device>? _deviceRepository;
        
        public IRepository<User> UserRepository => _userRepository ??= new UserRepository(_context);
        public IRepository<Device> DeviceRepository => _deviceRepository ??= new DeviceRepository(_context);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}