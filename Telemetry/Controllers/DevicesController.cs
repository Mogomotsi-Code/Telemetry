using Microsoft.AspNetCore.Mvc;
using Telemetry.Context.Repository.Interfaces;
using Telemetry.Domain.Tenancy.Interfaces;

namespace Telemetry.Api.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DevicesController : BaseController
    {
        private IDeviceRepository _deviceRepository;

        public DevicesController(IDeviceRepository deviceRepository, ITenantProvider tenantProvider)
            :base(tenantProvider) 
        {
            _deviceRepository = deviceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDevices()
        {
            var devices = await _deviceRepository.GetAllAsync();
            return Ok(devices);
        }
       
    }
}
