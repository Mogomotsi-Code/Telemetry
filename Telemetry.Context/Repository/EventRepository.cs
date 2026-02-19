using Telemetry.Context.Repository.Interfaces;
using Telemetry.Domain.Models;

namespace Telemetry.Context.Repository
{
    public class EventRepository : IEventyRepository
    {
        public Task<Event> CreateAsync(Event entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Event entity)
        {
            throw new NotImplementedException();
        }

        public Task<Event> GetByIdAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<Event> UpdateAsync(Event entity)
        {
            throw new NotImplementedException();
        }
    }
}
