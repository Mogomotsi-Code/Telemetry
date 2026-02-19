using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Context.DbContext
{
    public readonly record struct TenantContext(string CustomerId);
    
    public static TenantContext From(HttpRequest req)
    {
        var customerId =
            req.Headers["X-Customer-Id"].FirstOrDefault()
            ?? req.Query["customerId"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(customerId))
            throw new Exception("Missing tenant context. Provide X-Customer-Id header or customerId query param.");

        return new TenantContext(customerId);
    }
    
}
