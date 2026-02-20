using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Domain.Tenancy.Interfaces;

namespace Telemetry.Domain.Tenancy
{
    public class HeaderTenantProvider : ITenantProvider
    {
        public string CustomerId { get;  }
        public HeaderTenantProvider(IHttpContextAccessor accessor)
        {
            try
            {
                var req = accessor.HttpContext?.Request
                        ?? throw new InvalidOperationException("No active HTTP context.");

                CustomerId =
                    req.Headers["X-Customer-Id"].FirstOrDefault()
                    ?? req.Query["customerId"].FirstOrDefault()
                    ?? throw new Exception("Missing tenant context. Provide X-Customer-Id header or customerId query param.");
            }
            catch (Exception)
            {

                throw new Exception("Missing tenant context. Provide X-Customer-Id header or customerId query param.");
            }
        }
    }
}
