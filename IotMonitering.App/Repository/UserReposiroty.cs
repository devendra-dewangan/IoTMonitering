using IoTMonitering.Domain.Entity;
using IoTMonitoring.Data;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitoring.App.Repository;

public class UserRepository : IRepository<User>
{
    private AppDbContext _context;

    public UserRepository(AppDbContext appContext)
    {
        _context = appContext;
    }

    public async Task AddAsync(User entity)
    {
        await _context.AddAsync(entity);
    }

    public async Task DeleteAsync(string id)
    {
        _context.Remove(new User() { UserID = id });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return [.. _context.Users];
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserID == id);
    }

    public async Task UpdateAsync(User entity)
    {
        _context.Update(entity);
    }
}