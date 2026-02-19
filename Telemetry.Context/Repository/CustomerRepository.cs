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
    public class CustomerRepository : ICustomerRepository
    {
        private TelemetryDb _dbContext;
        public CustomerRepository(TelemetryDb dbContext) {
            _dbContext = dbContext;
        }
        Task<Response<Customer>> IRepository<Customer>.CreateAsync(Customer entity)
        {
            throw new NotImplementedException();
        }

        Task<Response<Customer>> IRepository<Customer>.DeleteAsync(Customer entity)
        {
            throw new NotImplementedException();
        }

        Task<Response<List<Customer>>> IRepository<Customer>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<Response<Customer>> IRepository<Customer>.GetByIdAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        Task<Response<Customer>> IRepository<Customer>.UpdateAsync(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
