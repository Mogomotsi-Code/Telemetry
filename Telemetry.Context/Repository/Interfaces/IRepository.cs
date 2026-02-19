using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Context.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(params object[] keyValues);
        Task<List<T>> GetAllAsync();
    }
}
