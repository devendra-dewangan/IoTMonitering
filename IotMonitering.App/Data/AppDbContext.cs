using IoTMonitering.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitoring.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Telemetry> Telemetries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(u => u.Devices)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

                entity.Property(u => u.UserID)
                        .IsRequired();
            });


            modelBuilder.Entity<Device>()
                .HasMany<Telemetry>()
                .WithOne(t => t.Device)
                .HasForeignKey(t => t.DeviceId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
