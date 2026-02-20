using Telemetry.Domain;
using Telemetry.Domain.Models;

namespace Telemetry.Context.Repository.Interfaces
{
    public interface IEventRepository : IRepository<Domain.Models.Event>
    {
        Task<Response<List<Event>>> GetDeviceEvents(string deviceId, DateTime start, DateTime end);

        Task<Response<Insight>> GetDeviceInsights(string deviceId, int windowHours);
    }
}
