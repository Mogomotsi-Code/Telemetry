namespace Telemetry.Domain.Dtos;

public record TelemetryIn(
    string? CustomerId,
    string DeviceId,
    string EventId,
    DateTime RecordedAt,
    string? Type,
    double Value,
    string? Unit
);
