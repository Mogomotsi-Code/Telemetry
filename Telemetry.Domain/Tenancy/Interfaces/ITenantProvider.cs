using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Domain.Tenancy.Interfaces
{
    public interface ITenantProvider
    {
        string CustomerId { get; }
    }
}
