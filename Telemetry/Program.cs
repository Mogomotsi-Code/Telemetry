using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Telemetry.Context.DbContext;
using Telemetry.Context.Repository;
using Telemetry.Context.Repository.Interfaces;
using Telemetry.Domain.Tenancy;
using Telemetry.Domain.Tenancy.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(_ => { });

// Use configured connection string (below). Removed malformed duplicate registration.

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantProvider, HeaderTenantProvider>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddDbContext<TelemetryDb>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")!));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options =>
{
    options.AddSecurityDefinition("TenantHeader", new OpenApiSecurityScheme
    {
        Name = "X-Customer-Id",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Tenant identifier. Example: acme-123 or beta-456"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "TenantHeader"
                }
            },
            Array.Empty<string>()
        }
    });
});

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

using (IServiceScope scope = app.Services.CreateScope())
{
    TelemetryDb db = scope.ServiceProvider.GetRequiredService<TelemetryDb>();
    await DbInitializer.MigrateAndSeedAsync(db);
}

app.UseHttpLogging();

app.Run();
