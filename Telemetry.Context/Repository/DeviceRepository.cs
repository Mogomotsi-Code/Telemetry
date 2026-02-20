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
using Telemetry.Domain.Tenancy;
using Telemetry.Domain.Tenancy.Interfaces;

namespace Telemetry.Context.Repository
{
    public class DeviceRepository : IDeviceRepository
    {
        private TelemetryDb _dbContext;
        private ITenantProvider _tenantProvider;
        public DeviceRepository(TelemetryDb dbContext, ITenantProvider tenantProvider)
        {
            _dbContext = dbContext;
            _tenantProvider = tenantProvider;
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
            return Task.Run(() =>
            {
                var devices = _dbContext.Devices
                    .Where(e => e.CustomerId == _tenantProvider.CustomerId)
                    .ToList();
                return new Response<List<Device>>
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Customer retrieved successfully.",
                    Payload = devices
                };
            });
        }

        public Task<Response<Device>> GetByIdAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<Device>>> GetCutomerDevices(string customerId)
        {
            return Task.Run(() =>
            {
                var devices = _dbContext.Devices
                    .Where(e => e.CustomerId == customerId)
                    .ToList();
                return new Response<List<Device>>
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Customer retrieved successfully.",
                    Payload = devices
                };
            });
        }

        public Task<Response<Device>> UpdateAsync(Device entity)
        {
            throw new NotImplementedException();
        }
    }
}
