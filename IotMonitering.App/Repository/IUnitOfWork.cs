using IoTMonitoring.Models;

namespace IoTMonitoring.App.Repository
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IDeviceRepository Devices { get; }
        Task<int> CommitAsync();
    }
}