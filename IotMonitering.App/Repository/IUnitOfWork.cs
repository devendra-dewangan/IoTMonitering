
using IoTMonitering.Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace IoTMonitoring.App.Repository
{
    public interface IUnitOfWork
    {
        IDeviceRepository Devices { get; }
        Task<int> CommitAsync();
    }
}