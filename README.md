# EFT Assistant

Local-first assistant for Escape from Tarkov. The current backend slice provides a resilient HTTP API over the public Tarkov.dev GraphQL API.

## Requirements

- .NET 10 SDK
- Docker Desktop for PostgreSQL and containerized startup

## Run locally

```powershell
docker compose up database -d
dotnet restore TarkovAssistant.sln
dotnet run --project src/TarkovAssistant.Backend
```

The API listens on `http://localhost:5080`. Opening this address in a browser redirects to the Scalar API reference at `http://localhost:5080/scalar/v1`. OpenAPI JSON is available at `http://localhost:5080/openapi/v1.json`.

## Run with Docker

```powershell
docker compose up --build
```

Docker maps backend host port `5080` to container port `8080` and PostgreSQL to host port `5432`. The backend waits for PostgreSQL and applies EF Core migrations during startup. Use `http://localhost:5080` from the host; `http://localhost:8080` is only the container's internal listening port and is not published directly.

## Endpoints

- `GET /` — redirects to the Scalar API reference
- `GET /scalar/v1` — interactive API reference
- `GET /health`
- `GET /api/tarkov/items`
- `GET /api/tarkov/items/{id}`
- `GET /api/tarkov/weapons`
- `GET /api/tarkov/ammo`
- `GET /api/tarkov/tasks`
- `GET /api/tarkov/traders`
- `GET /api/tarkov/hideout-stations`

Collection endpoints accept `language`, `gameMode`, `limit`, and `offset`. The items endpoint also accepts `itemType`. Supported game modes are `regular` and `pve`; page size is limited to 500 records.

Every matched endpoint writes a single structured completion event to the console with its method, path, endpoint name, status code, duration, and trace ID. Failures from Tarkov.dev are returned as RFC 9457 problem details. HTTP retries use exponential backoff with jitter and are bounded by attempt and total timeouts plus a circuit breaker.

Configuration can be overridden with environment variables:

```powershell
$env:TarkovDev__BaseUrl = "https://api.tarkov.dev/graphql"
$env:TarkovDev__UserAgent = "eft-assistant/1.0"
$env:ConnectionStrings__Database = "Host=localhost;Port=5432;Database=tarkov_assistant;Username=tarkov_assistant;Password=tarkov_assistant"
```

The checked-in connection string contains local-only development credentials. Override it when using a different local PostgreSQL instance.
