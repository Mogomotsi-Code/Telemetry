using Microsoft.EntityFrameworkCore;
using Telemetry.Context.DbContext;
using Telemetry.Context.Repository.Interfaces;
using Telemetry.Domain;
using Telemetry.Domain.Models;
using Telemetry.Domain.Tenancy.Interfaces;

namespace Telemetry.Context.Repository
{
    public class EventRepository : IEventRepository
    {
        private TelemetryDb _dbContext;
        private ITenantProvider _tenantProvider;
        public EventRepository(TelemetryDb dbContext, ITenantProvider tenantProvider)
        {
            _dbContext = dbContext;
            _tenantProvider = tenantProvider;
        }

        public Task<Response<Event>> CreateAsync(Event entity)
        {
            return Task.Run(() =>
            {
                _dbContext.TelemetryEvents.AddAsync(entity);
                _dbContext.SaveChangesAsync();
                return new Response<Event>
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Message = "Events retrieved successfully.",                    
                    Payload = { }                
                };
            });
        }

        public Task<Response<Event>> DeleteAsync(Event entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<Event>>> GetAllAsync()
        {
            return Task.Run(() =>
            {
                var events = _dbContext.TelemetryEvents
                    .Where(e => e.CustomerId == _tenantProvider.CustomerId)
                    .ToList();
                return new Response<List<Event>>
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Events retrieved successfully.",
                    Payload = events
                };
            });
        }

        public Task<Response<Event>> GetByIdAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<Event>>> GetDeviceEvents(string deviceId, DateTime start, DateTime end)
        {
            return Task.Run(async () =>
            {
                List<Event> events = await _dbContext.TelemetryEvents
                    .Where(e => e.CustomerId == _tenantProvider.CustomerId && e.DeviceId == deviceId)
                    .Where(e => e.RecordedAt >= start && e.RecordedAt <= end)
                    .OrderBy(e => e.RecordedAt)
                    .ToListAsync();
                return new Response<List<Event>>
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Device Events retrieved successfully.",
                    Payload = events
                };
            });
        }

        public  Task<Response<Insight>> GetDeviceInsights(string deviceId,int windowHours = 24)
        {
            return Task.Run(async () =>
            {
                var end = DateTime.UtcNow;
                var start = end.AddHours(-Math.Max(1, windowHours));

                var events = await _dbContext.TelemetryEvents
                    .Where(e => e.CustomerId == _tenantProvider.CustomerId && e.DeviceId == deviceId)
                    .Where(e => e.RecordedAt >= start && e.RecordedAt <= end)
                    .ToListAsync();

                var stats = events.GroupBy(_ => 1)
                    .Select(g => new { Min = g.Min(x => x.Value), Avg = g.Average(x => x.Value), Max = g.Max(x => x.Value) })
                    .FirstOrDefault();

                var latest = events.OrderByDescending(x => x.RecordedAt)
                    .Select(x => new { x.RecordedAt, x.Value, x.Unit, x.Type, x.EventId })
                    .FirstOrDefault();

                return new Response<Insight>
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Device Insights retrieved successfully.",
                    Payload = new Insight
                    {
                        Latest = events,
                        Stats = new Stat
                        {
                            Min = stats?.Min ?? 0,
                            Avg = stats?.Avg ?? 0,
                            Max = stats?.Max ?? 0
                        },
                         WindowHours = windowHours  
                    }
                };

            });
        }

        public Task<Response<Event>> UpdateAsync(Event entity)
        {
            throw new NotImplementedException();
        }
    }
}
