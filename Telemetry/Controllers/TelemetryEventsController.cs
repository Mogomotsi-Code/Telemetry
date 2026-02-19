using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Telemetry.Context.Repository.Interfaces;
using Telemetry.Domain.Dtos;
using Telemetry.Domain.Models;

namespace Telemetry.Api.Controllers
{
    [ApiController]
    [Route("api/telemetry")]
    public class TelemetryEventsController : Controller
    {
        private IEventRepository _eventRepository;

        public TelemetryEventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet("{deviceId}/telemetry")]
        public async Task<IActionResult> GetTelemetry(string deviceId, [FromQuery] DateTime? since, [FromQuery] DateTime? until)
        {
            var end = until ?? DateTime.UtcNow;
            var start = since ?? end.AddHours(-24);

            var events = await _db.TelemetryEvents
                .Where(e => e.CustomerId == _tenant.CustomerId && e.DeviceId == deviceId)
                .Where(e => e.RecordedAt >= start && e.RecordedAt <= end)
                .OrderBy(e => e.RecordedAt)
                .ToListAsync();

            return Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> Ingest([FromBody] TelemetryIn dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.CustomerId) && dto.CustomerId != _tenant.CustomerId)
                return BadRequest(new { error = "customerId mismatch with tenant context" });

            var deviceExists = await _db.Devices.AnyAsync(d => d.CustomerId == _tenant.CustomerId && d.DeviceId == dto.DeviceId);
            if (!deviceExists) return NotFound(new { error = "device not found" });

            var entity = new Event
            {
                CustomerId = _tenant.CustomerId,
                DeviceId = dto.DeviceId,
                EventId = dto.EventId,
                RecordedAt = dto.RecordedAt.ToUniversalTime(),
                Type = dto.Type ?? "temperature",
                Value = dto.Value,
                Unit = dto.Unit ?? "C"
            };

            await _eventRepository.CreateAsync(entity);

            try
            {
                await _db.SaveChangesAsync();
                return Created("", new { inserted = true });
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message?.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) == true)
            {
                return Ok(new { inserted = false, duplicate = true });
            }
        }
    }
}
