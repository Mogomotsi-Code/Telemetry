using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Context.DbContext;
using Telemetry.Context.Repository.Interfaces;
using Telemetry.Domain;
using Telemetry.Domain.Models;

namespace Telemetry.Context.Repository
{
    public class DeviceRepository : IDeviceRepository
    {
        private TelemetryDb _dbContext;
        public DeviceRepository(TelemetryDb dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Response<Device>> CreateAsync(Device entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Device>> DeleteAsync(Device entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<Device>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Response<Device>> GetByIdAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Device>> UpdateAsync(Device entity)
        {
            throw new NotImplementedException();
        }
    }
}
