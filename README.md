# FarmSim backend

This workspace contains the ASP.NET Core backend and its performance tests. It intentionally does not set up the mobile application or Git/GitHub workflow.

## What is implemented

- .NET 9 ASP.NET Core controller API
- Scalar API reference at `http://localhost:5000/scalar/v1`
- OpenAPI document at `http://localhost:5000/openapi/v1.json`
- in-memory, thread-safe hydroponics sensor simulation
- in-memory actuator state simulation
- validation responses using RFC 7807 Problem Details
- xUnit integration tests
- k6 smoke and assignment load-test profiles

Swagger UI is not installed or mapped.

## Run on D:

All project files, build output, NuGet packages, temporary CLI state, the portable k6 binary, and test results are kept under `D:\FarmSim`. The existing system-wide .NET runtime remains in its installed location and is not duplicated.

From PowerShell:

```powershell
Set-Location D:\FarmSim
.\run-backend.cmd
```

Then open Scalar:

```text
http://localhost:5000/scalar/v1
```

## Test

Run the integration tests:

```powershell
.\test-backend.cmd
```

With the backend running in another terminal, run a short k6 check:

```powershell
.\run-k6.cmd -Profile smoke
```

Run the full assignment profile (1 minute ramp up, 3 minutes at 50 users, 1 minute ramp down):

```powershell
.\run-k6.cmd -Profile load
```

The k6 scripts enforce a failure rate below 1% and p95 response time below 200 ms. Each run writes its terminal output and JSON summary to `docs\performance-testing\results`.

## API routes

| Method | Route | Purpose |
|---|---|---|
| GET | `/health` | Backend availability and service metadata |
| GET | `/api/sensors/current` | Generate the current simulated reading |
| POST | `/api/sensors/simulate` | Generate a reading with optional overrides |
| GET | `/api/sensors/history?limit=25` | Return newest generated readings |
| GET | `/api/actuators` | List simulated actuator states |
| POST | `/api/actuators/{name}` | Update a known actuator |

