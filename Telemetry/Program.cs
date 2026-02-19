using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Telemetry.Context.DbContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(_ => { });

builder.Services.AddDbContext<TelemetryDb>(opt =>
    opt.UseSqlite("Data SourceTelemetry.db"));

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz");

app.UseHttpLogging();

app.Run();
