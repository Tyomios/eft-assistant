# AGENTS.md

## Project purpose

This repository contains a local-first assistant for Escape from Tarkov. Its first business scenario is inventory assistance: while the player is viewing the stash, the Windows client observes the mouse position, recognizes the item under the cursor from screen pixels, and displays a non-interactive overlay with the item's identity, approximate value, usefulness, and a keep/sell/discard recommendation.

The local backend imports and normalizes public Tarkov reference data. It is not on the latency-critical path for mouse-hover recognition; the client synchronizes a versioned catalog and evaluates items locally.

## Current MVP scope

Implement only the following unless the user explicitly expands scope:

- A local ASP.NET Core backend.
- PostgreSQL running locally through Docker Compose.
- Background import of items, images, prices, quests, weapons, calibers, and ammunition compatibility from the Tarkov.dev API.
- A native C# Windows client running outside Docker.
- Detection of the game window and mouse position.
- Screen capture of a small region around the hovered inventory cell.
- Recognition of inventory items using locally cached catalog data.
- A click-through overlay showing a basic item or weapon recommendation.
- Manual data-import triggering and import-status inspection.

The first supported UI context is the stash. Traders and weapon inspection may be added after the stash flow works reliably. In-raid assistance is out of scope.

## Explicit non-goals for the current cycle

Do not introduce these without an explicit request:

- Cloud deployment or production hosting.
- CI/CD pipelines.
- Automated test projects or test infrastructure.
- Sentry, centralized logging, metrics, tracing, or telemetry.
- Authentication, accounts, or cloud profile synchronization.
- Redis, message brokers, Kubernetes, or microservices.
- Automatic mouse or keyboard input.
- Full-stash scanning.
- Complete weapon-build recognition from a single inventory icon.
- LLM-based item scoring.

Normal local build and manual verification commands are still expected when implementing changes.

## Required technology choices

### Backend

- C# and ASP.NET Core Web API on the current supported .NET LTS release.
- Entity Framework Core with `Npgsql.EntityFrameworkCore.PostgreSQL`.
- PostgreSQL as the only database.
- ASP.NET Core `BackgroundService` for scheduled imports during the MVP.
- Built-in OpenAPI support for local API exploration.
- ASP.NET Core MVC controllers for business HTTP endpoints. Do not introduce Minimal API business endpoints unless explicitly requested; infrastructure mappings such as OpenAPI, Scalar, and health checks are acceptable.
- Docker Compose for the backend and PostgreSQL.
- A single backend deployment unit. Do not split imports into a separate service unless there is a demonstrated need.

### Windows client

- C# and .NET.
- WPF by default for the desktop shell and transparent click-through overlay. Use WinUI 3 only if an existing implementation already establishes it.
- `Windows.Graphics.Capture` with Direct3D interop for screen capture.
- OpenCvSharp for deterministic image processing.
- SQLite for locally synchronized catalog and user state when persistence is needed.
- Start recognition with inventory geometry, image normalization, perceptual hashing, and feature/template matching.
- Add an ONNX model only after deterministic recognition has measurable failure cases and a usable labeled dataset.

The Windows client must not run in Docker because it needs access to the game window, cursor, screen capture APIs, and desktop composition.

## Architecture principles

- Keep the solution a modular monolith.
- Organize C# code by business feature, not by technical layers such as `Controllers`, `Services`, or `Repositories` at the repository root.
- Keep controller, request, response, handler, validation, and feature-specific persistence code close together.
- Prefer vertical slices with small explicit types over generic repository and service abstractions.
- Do not create speculative abstractions for future deployment, providers, or scale.
- Share API contracts deliberately, but never expose EF Core entities as transport contracts.
- Keep recognition and recommendation available offline after catalog synchronization.
- Do not call the backend on every mouse hover.
- Use stable internal item IDs and store external source IDs separately.
- Version synchronized catalog packages so the client can skip unchanged data.
- Keep recommendation rules deterministic and explainable.

## Expected repository layout

Use this layout as a direction, adapting only when existing code requires it:

```text
src/
  TarkovAssistant.Backend/
    Features/
      Catalog/
      Prices/
      Quests/
      Weapons/
      DataImport/
    Database/
    Configuration/
    Program.cs

  TarkovAssistant.Client/
    Features/
      BackendConnection/
      CatalogSync/
      GameWindowDetection/
      CursorTracking/
      ScreenCapture/
      InventoryGridDetection/
      ItemRecognition/
      ItemEvaluation/
      ItemOverlay/
    Database/
    Settings/

docker-compose.yml
```

Within a feature, keep the resource controller and its use-case types together, for example:

```text
Features/Catalog/
  ItemsController.cs
  GetItemRequest.cs
  GetItemResponse.cs
  GetItemHandler.cs
```

Keep exactly one declared C# type per file and name the file after that type. Keep related files together inside the owning feature folder rather than combining types into a single file.

## Local data flow

1. Docker Compose starts the ASP.NET Core backend and PostgreSQL.
2. The backend applies the expected local schema and runs or schedules the Tarkov.dev import.
3. Imported data is validated, normalized, and upserted idempotently.
4. The backend exposes a catalog version and versioned catalog data.
5. The Windows client synchronizes and caches the catalog locally.
6. When the cursor remains over an inventory item for roughly 200-300 ms, the client captures only the relevant screen region.
7. The client identifies the inventory cell, normalizes its icon, and resolves candidate item IDs.
8. The local evaluation feature combines price, quest requirements, hideout usefulness, and weapon/ammunition information.
9. A non-activating click-through overlay displays the result and recognition confidence.
10. Moving the cursor away hides the overlay.

## Backend API direction

Keep the initial API small. Typical endpoints are:

```text
GET  /api/catalog/version
GET  /api/catalog/items
GET  /api/catalog/items/{id}
GET  /api/items/{id}/price
GET  /api/items/{id}/quest-requirements
GET  /api/weapons/{id}/ammunition
POST /api/data-import/run
GET  /api/data-import/status
GET  /health
```

Endpoint names may evolve with the implementation, but bulk catalog synchronization is preferred over chatty per-item client traffic.

## Data modeling rules

- Use a generated internal UUID or ULID as the primary key.
- Store `ExternalSource` and `ExternalId` separately and enforce uniqueness on that pair.
- Preserve source timestamps when available.
- Make imports repeatable and safe to retry.
- Do not silently replace valid data with empty or incomplete upstream responses.
- Store normalized relationships for quests, item requirements, weapons, magazines, calibers, and ammunition compatibility.
- JSON columns are acceptable for source payload snapshots or genuinely variable attributes, not as a substitute for core relational modeling.
- Record enough import state to show last attempt, last successful import, current status, and a concise failure reason. Do not add a full observability stack.

## Recognition rules

- Perform recognition from screen pixels only.
- Trigger work after cursor dwell instead of continuously processing the full screen.
- Crop the smallest useful region and keep expensive work off the UI thread.
- Account for Windows display scaling, game resolution, UI scale, and window position.
- Return an item ID, confidence, and alternative candidates rather than only a display name.
- Do not show a strong recommendation below the configured confidence threshold.
- Make uncertainty visible to the user.
- Keep screenshots local by default and do not persist them unless the user explicitly enables data collection.
- Treat recognition of weapon condition, loaded ammunition, and installed modifications as unavailable unless those properties are visible on a dedicated inspection screen.

## Recommendation rules

Recommendations must be deterministic. The first version may use:

- Current trader and market value.
- Current and near-future quest requirements.
- Found-in-raid requirements.
- Hideout requirements when available.
- Item footprint and value per inventory cell.
- Trader availability and rarity.
- For weapons: caliber, useful compatible ammunition, ammunition availability, and base value.

Always expose the main reasons behind a recommendation. Avoid presenting approximate market data as an exact guaranteed sale price.

## Game-integrity and safety constraints

These constraints are mandatory:

- Never read or write Escape from Tarkov process memory.
- Never inject DLLs or code into the game.
- Never hook the game's rendering pipeline.
- Never modify game files or network traffic.
- Never synthesize mouse or keyboard input.
- Never attempt to bypass, hide from, or interfere with BattlEye.
- Use documented Windows screen-capture and windowing APIs from a separate process.
- Keep the overlay informational, non-interactive while shown, and click-through.

If a requested change approaches these boundaries, stop and explain the risk before implementing it. Public release should be preceded by explicit confirmation from BSG/BattlEye that the chosen behavior is permitted.

## Implementation discipline

- Inspect existing code and preserve established conventions before adding new ones.
- Keep changes focused on the requested vertical slice.
- Prefer framework features and small dependencies.
- Never commit secrets, Tarkov.dev credentials, local database passwords, captured screenshots, or generated user inventory data.
- Put safe local defaults in configuration and allow overrides through environment variables.
- Ensure Docker services are reachable from the host Windows client through documented localhost ports.
- Update local setup documentation whenever startup commands, ports, migrations, or required configuration change.
- Do not add infrastructure merely because it may be useful later.

## C# documentation and code-quality baseline

These rules apply to all new and modified C# code:

- Use explicit accessibility modifiers on every declared type and member, including interface members.
- Keep exactly one class, record, struct, interface, or enum per file. The filename must match the declared type.
- Add concise XML documentation to every public type and public member. Document parameters, return values, generic parameters, and relevant exceptions when they are not self-evident.
- Use `<inheritdoc />` for interface implementations when the interface documentation is sufficient. Do not duplicate the same prose in the implementation.
- XML documentation should describe the contract, units, nullability, side effects, and important constraints. Avoid filler such as restating the member name without adding meaning.
- Use inline comments only to explain non-obvious intent, external constraints, workarounds, or safety decisions. Do not narrate straightforward code.
- Prefer clear names and small explicit methods over explanatory comments. Remove stale comments whenever behavior changes.
- Minimize public surface area. Keep implementation details `internal` or `private`; do not make a type public only to simplify dependency injection or serialization.
- Preserve compiler-enforced XML documentation, latest-recommended analyzers, code-style enforcement, and warnings-as-errors in the project configuration.
- Do not suppress warnings globally. Fix the cause or add the narrowest possible suppression with a concrete justification.
- Use source-generated logging for repeated or performance-sensitive log events. Keep event IDs stable and log templates structured.

## Efficient implementation loop

Use this sequence to catch structural mistakes before they spread across many files:

1. Read `AGENTS.md`, `.editorconfig`, the project file, and one nearby representative feature before designing changes.
2. Decide the public contract, file boundaries, accessibility, and XML documentation before writing the implementation.
3. Implement one representative vertical slice first and run a Release build. Apply any analyzer feedback to the pattern before copying it to other slices.
4. After each structural batch, run `dotnet build TarkovAssistant.sln -c Release`. Do not postpone compilation until the full feature is written.
5. Before delivery, run whitespace, style, and analyzer verification at `info` severity, a vulnerable-package audit, and `git diff --check`.
6. Verify that every public declaration has XML documentation and every C# file declares at most one type.
7. For endpoint or hosting changes, test the real process boundary: local run, Docker port mapping, root URL, health, OpenAPI/Scalar, one successful request, and one invalid request.
8. Update README startup URLs and host-to-container port mappings in the same change.

Treat the existing `.editorconfig` and project analyzer settings as executable requirements. Do not weaken them to make a build pass.

Use these standard verification commands from the repository root:

```powershell
dotnet build TarkovAssistant.sln -c Release
dotnet format whitespace TarkovAssistant.sln --verify-no-changes
dotnet format style TarkovAssistant.sln --verify-no-changes --severity info
dotnet format analyzers TarkovAssistant.sln --verify-no-changes --severity info
dotnet list src/TarkovAssistant.Backend/TarkovAssistant.Backend.csproj package --vulnerable --include-transitive
docker compose config --quiet
git diff --check
```

## MVP completion target

The first end-to-end milestone is complete when a developer can:

1. Start the backend and PostgreSQL locally with Docker Compose.
2. Import a usable Tarkov catalog into PostgreSQL.
3. Launch the native Windows client.
4. Open the Tarkov stash in a supported resolution and UI scale.
5. Hover over a supported inventory item.
6. See its recognized name, approximate value, confidence, and basic recommendation within about 500 ms after cursor dwell.
7. Continue receiving recommendations from the local cache when the backend is temporarily unavailable.
