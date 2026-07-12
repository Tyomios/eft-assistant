# EFT Assistant

Local-first assistant for Escape from Tarkov. The current backend slice provides a resilient HTTP API over the public Tarkov.dev GraphQL API.

## Requirements

- .NET 10 SDK
- Docker Desktop for PostgreSQL and containerized startup
- Windows 10 or later for the native WPF client

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

## Windows client

The native client is a separate WPF application and is not part of Docker Compose:

```powershell
dotnet run --project src/TarkovAssistant.Client
```

The current client is a runnable desktop shell. Its `BackendConnection` feature defines the future connection boundary and uses `http://localhost:5080` as the safe local default, but it does not send HTTP requests yet.

Catalog images are read beneath `%LocalAppData%\TarkovAssistant\Catalog`. The file reader rejects paths outside that boundary, limits individual image size, and can validate SHA-256 hashes supplied by the synchronized catalog. Missing, unreadable, oversized, and hash-mismatched files are returned as classified results.

The client detects the visible top-level window owned by `EscapeFromTarkov.exe` through documented Windows windowing APIs. It records physical-pixel bounds, foreground state, and a non-capturable minimized state without accessing game memory or injecting into the game. The shell exposes a manual detection check.

The cursor tracker uses `GetCursorPos` to poll physical virtual-screen coordinates every 50 ms only while manually enabled. It resets when Tarkov is unavailable, minimized, backgrounded, or the cursor leaves the game window. After 250 ms within a three-pixel tolerance, it emits one recognition trigger and does not repeat it until the cursor moves or tracking is reset.

The client captures a small 384×384 physical-pixel BGRA32 region around a dwell-satisfied cursor through `Windows.Graphics.Capture`. The capture target is the Tarkov HWND, never the full desktop; the client copies only the small cursor region from the D3D11 frame and keeps no screenshots. It detects a repeated stash-cell grid from local pixel edges, adapts to the current client bounds, DPI, window position, resolution, and UI scale, and rejects ambiguous UI areas and empty cells. Closing, minimizing, resizing, or Windows refusing a capture request returns a classified result and the next cursor dwell starts a fresh request.

The client also contains the contract for recognizing a hovered item from the captured BGRA32 region and a non-activating click-through WPF overlay. The shell exposes capture and stash-cell detection in the cursor-tracking status; catalog image matching, local evaluation, and recommendation content remain separate feature slices.
