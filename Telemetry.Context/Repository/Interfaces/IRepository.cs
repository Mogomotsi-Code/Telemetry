using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Domain;

namespace Telemetry.Context.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<Response<T>> CreateAsync(T entity);
        Task<Response<T>> UpdateAsync(T entity);
        Task<Response<T>> DeleteAsync(T entity);
        Task<Response<T>> GetByIdAsync(params object[] keyValues);
        Task<Response<List<T>>> GetAllAsync();
    }
}
