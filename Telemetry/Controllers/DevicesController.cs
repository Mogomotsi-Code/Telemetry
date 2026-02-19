using Microsoft.AspNetCore.Mvc;
using Telemetry.Context.Repository.Interfaces;

namespace Telemetry.Api.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DevicesController : Controller
    {
        private IDeviceRepository _deviceRepository;

        public DevicesController(IDeviceRepository deviceRepository)
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
