using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Context.DbContext;
using Telemetry.Context.Repository.Interfaces;
using Telemetry.Domain.Models;

namespace Telemetry.Context.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private TelemetryDb _dbContext;
        public CustomerRepository(TelemetryDb dbContext) {
            _dbContext = dbContext;
        }
        public Task<Customer> CreateAsync(Customer entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Customer entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Customer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetByIdAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> UpdateAsync(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
