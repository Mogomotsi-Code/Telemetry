using Microsoft.EntityFrameworkCore;
using Telemetry.Context.DbContext;
using Telemetry.Context.Repository.Interfaces;
using Telemetry.Domain;
using Telemetry.Domain.Models;

namespace Telemetry.Context.Repository
{
    public class EventRepository : IEventRepository
    {
        private TelemetryDb _dbContext;
        private TenantContext _tenantContext;
        public EventRepository(TelemetryDb dbContext, TenantContext tenantContext)
        {
            _dbContext = dbContext;
            _tenantContext = tenantContext;
        }

        public Task<Response<Event>> CreateAsync(Event entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Event>> DeleteAsync(Event entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<Event>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Response<Event>> GetByIdAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Event>> UpdateAsync(Event entity)
        {
            throw new NotImplementedException();
        }
    }
}
