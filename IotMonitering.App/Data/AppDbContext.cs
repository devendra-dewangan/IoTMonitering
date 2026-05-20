using IoTMonitering.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitoring.Data
{
    public class AppDbContext
        : IdentityDbContext<User, IdentityRole, string>
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Device>()
                .HasKey(d => d.Id);
    
            modelBuilder.Entity<User>()
                .HasMany(t => t.Devices)
                .WithOne(d => d.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}