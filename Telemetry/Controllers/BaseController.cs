using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Telemetry.Domain.Tenancy.Interfaces;

namespace Telemetry.Api.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ITenantProvider _tenantProvider;
        public BaseController(ITenantProvider tenantProvider) {
            _tenantProvider = tenantProvider;
        }
    }
}
