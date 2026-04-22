using IoTMonitoring.Models;

namespace IoTMonitoring.App.Repository
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }
        IRepository<Device> DeviceRepository { get; }
        Task SaveAsync();
    }
}