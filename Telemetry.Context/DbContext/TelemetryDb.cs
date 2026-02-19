using Microsoft.EntityFrameworkCore;
using Telemetry.Domain.Models;

namespace Telemetry.Context.DbContext
{
    public class TelemetryDb : Microsoft.EntityFrameworkCore.DbContext
    {
        public TelemetryDb(DbContextOptions<TelemetryDb> opts) : base(opts) { }
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Device> Devices => Set<Device>();
        public DbSet<Event> TelemetryEvents => Set<Event>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Customer>().HasKey(x => x.CustomerId);

            b.Entity<Device>().HasKey(x => new { x.CustomerId, x.DeviceId });

            b.Entity<Event>().HasKey(x => new { x.CustomerId, x.DeviceId, x.EventId });
            b.Entity<Event>()
                .HasIndex(x => new { x.CustomerId, x.DeviceId, x.RecordedAt });

            base.OnModelCreating(b);
        }
    }
}
