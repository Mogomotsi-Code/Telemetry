using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Telemetry.Context.Repository.Interfaces;
using Telemetry.Domain.Dtos;
using Telemetry.Domain.Models;
using Telemetry.Domain.Tenancy.Interfaces;

namespace Telemetry.Api.Controllers
{
    [ApiController]
    [Route("api/telemetry")]
    public class TelemetryEventsController : BaseController
    {
        private IEventRepository _eventRepository;
        private IDeviceRepository _deviceRepository;

        public TelemetryEventsController(IEventRepository eventRepository, IDeviceRepository deviceRepository, ITenantProvider tenantProvider)
            : base(tenantProvider)
        {
            _eventRepository = eventRepository;
            _deviceRepository = deviceRepository;
        }

        [HttpGet("{deviceId}/telemetry")]
        public async Task<IActionResult> GetTelemetry(string deviceId, [FromQuery] DateTime? since, [FromQuery] DateTime? until)
        {
            var end = until ?? DateTime.UtcNow;
            var start = since ?? end.AddHours(-24);

            var results = await _eventRepository.GetDeviceEvents(deviceId, start,end);
            if(results.StatusCode == HttpStatusCode.OK)
                return Ok(results);
            return BadRequest(new { error = "" });
        }

        [HttpPost]
        public async Task<IActionResult> Ingest([FromBody] TelemetryIn dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.CustomerId) && dto.CustomerId != _tenantProvider.CustomerId)
                return BadRequest(new { error = "customerId mismatch with tenant context" });

            var results = await _deviceRepository.GetCutomerDevices(dto?.CustomerId);
               
            if (results.StatusCode != HttpStatusCode.OK || !results.Payload.Any(d => d.DeviceId.Equals(dto.DeviceId))) return NotFound(new { error = "device not found" });

            var entity = new Event
            {
                CustomerId = _tenantProvider.CustomerId,
                DeviceId = dto.DeviceId,
                EventId = dto.EventId,
                RecordedAt = dto.RecordedAt.ToUniversalTime(),
                Type = dto.Type ?? "temperature",
                Value = dto.Value,
                Unit = dto.Unit ?? "C"
            };

            var response = await _eventRepository.CreateAsync(entity);
            if(response.StatusCode == HttpStatusCode.Created)
                return Created("", new { inserted = true });

            return BadRequest(new { error = "" });
        }

        [HttpGet("{deviceId}/insights")]
        public async Task<IActionResult> GetInsights(string deviceId, [FromQuery] int windowHours = 24)
        {
            var end = DateTime.UtcNow;
            var start = end.AddHours(-Math.Max(1, windowHours));
            var results = await _eventRepository.GetDeviceInsights(deviceId, windowHours);

            if (results.StatusCode == HttpStatusCode.OK)
                return  Ok(results);

            return BadRequest(new { error = "" });


        }
    }
}
