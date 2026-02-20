using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Domain.Models;

namespace Telemetry.Context.DbContext
{
    public static class DbInitializer
    {
        public static async Task MigrateAndSeedAsync(TelemetryDb db)
        {
            // For quick dev/testing create DB/schema from the model if it doesn't exist.
            // This avoids requiring EF migrations when running locally.
            await db.Database.EnsureCreatedAsync();

            if (await db.Customers.AnyAsync()) return;

            db.Customers.AddRange(
                new Customer { CustomerId = "acme-123" },
                new Customer { CustomerId = "beta-456" }
            );

            db.Devices.AddRange(
                new Device { CustomerId = "acme-123", DeviceId = "dev-001", Label = "Boiler #3", Location = "Plant A" },
                new Device { CustomerId = "acme-123", DeviceId = "dev-002", Label = "Chiller #1", Location = "Plant A" },
                new Device { CustomerId = "beta-456", DeviceId = "dev-100", Label = "Pump #9", Location = "Site B" }
            );

            db.TelemetryEvents.AddRange(
                new Event { CustomerId = "acme-123", DeviceId = "dev-001", EventId = "evt-a1", RecordedAt = DateTime.Parse("2025-05-04T12:34:56Z").ToUniversalTime(), Type = "temperature", Value = 21.5, Unit = "C" },
                new Event { CustomerId = "acme-123", DeviceId = "dev-001", EventId = "evt-a2", RecordedAt = DateTime.Parse("2025-05-04T12:35:30Z").ToUniversalTime(), Type = "temperature", Value = 22.0, Unit = "C" },
               // Out of order arrival
                new Event { CustomerId = "acme-123", DeviceId = "dev-001", EventId = "evt-a0", RecordedAt = DateTime.Parse("2025-05-04T12:30:00Z").ToUniversalTime(), Type = "temperature", Value = 21.0, Unit = "C" },
                new Event { CustomerId = "acme-123", DeviceId = "dev-002", EventId = "evt-b1", RecordedAt = DateTime.Parse("2025-05-04T12:40:00Z").ToUniversalTime(), Type = "temperature", Value = 6.8, Unit = "C" },
                new Event { CustomerId = "beta-456", DeviceId = "dev-100", EventId = "evt-c1", RecordedAt = DateTime.Parse("2025-05-04T13:00:00Z").ToUniversalTime(), Type = "temperature", Value = 55.2, Unit = "C" }
            );

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // If you keep the duplicate seed entry, the save will fail.
                // Best approach for seed: don't include the duplicate here,
                // and instead test duplicates via POST /api/telemetry.
                throw;
            }
        }
    }
}
