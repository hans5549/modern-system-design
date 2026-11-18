# C# coding style

Enforced where possible by `.editorconfig` and `Directory.Build.props` (warnings are errors).
Basis: Microsoft C# identifier-name guidance and the .NET runtime coding style.

## Language features (C# 14 / .NET 10)

- File-scoped namespaces. The namespace matches the folder path.
- One top-level type per file; the file is named after the type.
- Always use braces, even for one-line bodies.
- Nullable reference types are on. Do not silence them with `!` without a comment saying why it is safe.
- Prefer primary constructors for dependency injection. Do not copy parameters into fields unless needed.
- `record` + `init` for immutable data, `class` for behavior. Mark classes `sealed` unless designed for inheritance.
- Use collection expressions (`[]`), expression-bodied members for single expressions, and `is not null`.
- `var` when the type is obvious from the right-hand side; otherwise the explicit type.
- Expose collections as `IReadOnlyList<T>` / `IReadOnlyCollection<T>`.

## Async

- Async all the way: no `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()`.
- No `async void`, except framework-required event handlers. Razor handlers return `Task`.
- Accept a `CancellationToken` and forward it through service, repository, and EF/Dapper (`CommandDefinition`).
- No `ConfigureAwait(false)` in app code (ASP.NET Core has no synchronization context).

## Errors and validation

- Validate at trust boundaries (UI input, configuration, external data), not on every internal call.
- Guard with `ArgumentException.ThrowIfNullOrWhiteSpace` and `ArgumentNullException.ThrowIfNull`.
- Throw specific exception types. Never swallow with a bare `catch (Exception)`; catch only to handle,
  translate, or log and rethrow.
- Do not use exceptions for expected control flow.

## Data access

- Dapper: always parameterized SQL. Never concatenate or interpolate user values into SQL.
- EF Core: short-lived contexts from `IDbContextFactory<AppDbContext>`; `AsNoTracking()` for EF read queries.
- SQL and EF calls live in repositories only.

## Logging

- Inject `ILogger<T>`. Use message templates with named placeholders:
  `logger.LogInformation("Added note {NoteId}", id)`. Never interpolate strings in log calls.
- Do not log secrets or raw user input.

## Comments

- English. Explain why, not what. XML docs on public APIs only when behavior is non-obvious.
- Known shortcuts use this form: `// shortcut: <the limit>, <when to upgrade>`.
- No commented-out code and no `#region`.
