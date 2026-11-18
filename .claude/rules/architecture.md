# Architecture

Single project, layered. Split into multiple projects only for a concrete reason, for example a second host
that reuses the same domain, or build times that hurt.

## Layers and dependency direction

```
Components (Razor) -> Services -> Repositories -> Data (AppDbContext, SQLite)
        Models (shared data types) are used by every layer and depend on none
```

Dependencies point forward only. A layer never references a layer before it.

| Layer | Folder | Responsibility | Must not |
|---|---|---|---|
| Components | `Components/` | UI, user input, render state | inject repositories or `AppDbContext`; hold business rules or SQL |
| Services | `Services/` | Business rules, validation, orchestrating repositories | contain SQL or EF calls; reference Blazor or HTTP types |
| Repositories | `Repositories/` | All data access: EF Core writes, Dapper reads | hold business rules; return tracked entities across calls |
| Data | `Data/` | `AppDbContext` and EF configuration | contain feature queries |
| Models | `Models/` | Entities and DTOs | depend on any other layer |

## Conventions

- One interface per service and repository, in the same folder as its implementation (`INoteService` next to
  `NoteService`). Interfaces exist as DI and testing seams, not for every class.
- Blazor Server: use `IDbContextFactory<AppDbContext>`. A scoped context lives as long as the circuit.
  Create, use, and dispose a context per repository call.
- DI lifetimes: services and repositories `Scoped`; stateless helpers `Singleton`; avoid `Transient` without a
  reason. Do not resolve services from `IServiceProvider` (service locator).
- Configuration: `appsettings*.json` plus environment overrides. Bind sections to typed options (`IOptions<T>`)
  once there is more than a connection string. No secrets in committed files; use user-secrets or env vars.
- `Program.cs` is the composition root only. When it grows past about 60 lines, move registrations into
  `AddXxx` extension methods per concern (persistence, logging, ...).
- Entity vs DTO: one `Note` type is fine while they are identical. Split when the UI shape diverges from the table.
- Persistence: `EnsureCreated()` is a recorded shortcut (delete `app.db` when the model changes). Move to
  EF Core migrations once the data must survive schema changes.
- Services report failures as exceptions; the UI decides how to present them. Unhandled exceptions go to
  Serilog request logging and the error page.

## Adding a feature

1. Model/entity (plus `DbSet` if persisted).
2. Repository interface, then implementation (EF write, Dapper read).
3. Service interface, then implementation (rules, validation).
4. Register both in `Program.cs`.
5. Component calls the service only.
6. Tests: service unit tests, repository integration test.
7. Self-review against the checklist in `code-review.md`.
