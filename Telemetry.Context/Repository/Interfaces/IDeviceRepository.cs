using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Domain;
using Telemetry.Domain.Models;

namespace Telemetry.Context.Repository.Interfaces
{
    public interface IDeviceRepository : IRepository<Domain.Models.Device>
    {
        Task<Response<List<Device>>> GetCutomerDevices(string customerId);
    }
}
