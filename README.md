# Telemetry SaaS Slice

A minimal multi-tenant telemetry ingestion and exploration system built with:

- ASP.NET Core 8
- SQLite
- React 18 + TypeScript
- Redux Toolkit + RTK Query

This project demonstrates tenant isolation, idempotent event ingestion, out-of-order handling, and basic telemetry insights.

---


---

# 🚀 Running the Project Locally

## 1️⃣ Backend

### Requirements
- .NET 8 SDK

### Restore + Run
```bash
dotnet restore
dotnet run --project Telemetry.API


The API will run on https://localhost:7284/

Swagger UI: https://localhost:7284/swagger/index.html

```
## 1️⃣ Frontend
```bash
npm install
npm run dev
```
Tenant header can be set in:

- Swagger (Authorize → X-Customer-Id)

- Frontend (automatically via RTK Query)

- Postman

# 🧠 Frontend Architecture

State management:

- Redux Toolkit

- RTK Query

RTK Query handles:

- Data fetching

- Caching

- Loading state

- Error state

- Cache invalidation

Tenant switching updates API headers automatically.

# 🔐 Tenant Isolation

- Tenant resolved from X-Customer-Id

- All queries filtered by CustomerId

- Writes validate device ownership

- Duplicate events prevented by DB primary key

# 📊 API Endpoints
Devices
 - GET /api/devices

Telemetry (last 24h)
- GET /api/devices/{deviceId}/telemetry

Insights
- GET /api/devices/{deviceId}/insights

Ingest
- POST /api/telemetry

# 🩺 Health Check
GET /healthz