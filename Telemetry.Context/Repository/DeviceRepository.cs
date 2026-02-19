using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Context.Repository.Interfaces;
using Telemetry.Domain.Models;

namespace Telemetry.Context.Repository
{
    public class DeviceRepository : IDeviceRepository
    {
        public Task<Device> CreateAsync(Device entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Device entity)
        {
            throw new NotImplementedException();
        }

        public Task<Device> GetByIdAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<Device> UpdateAsync(Device entity)
        {
            throw new NotImplementedException();
        }
    }
}
