---
paths:
  - "**/*.razor"
  - "**/*.razor.cs"
---

# Blazor rules

Render mode is global `InteractiveServer` (set on `Routes` and `HeadOutlet` in `App.razor`).
Components run on the server inside a circuit, so state lives in server memory per user connection.

- Directive order at the top of the file: `@page`, `@using`, `@implements`, `@inject`; then markup; then `@code`.
- Components handle UI only. Inject `I<Entity>Service`, never repositories or `AppDbContext`
  (see `architecture.md`).
- Load data in `OnInitializedAsync` (or `OnParametersSetAsync` when it depends on parameters). Do not block
  with `.Result` / `.Wait()`.
- Event handlers return `Task` (never `async void`). Name them `HandleXxxAsync`.
- Use `@key` on items rendered in loops when identity matters.
- Call `StateHasChanged` only when state changes outside a Blazor event or lifecycle method.
- Implement `IAsyncDisposable` / `IDisposable` for timers, event subscriptions, and `CancellationTokenSource`.
- Forms: `EditForm` with a model and DataAnnotations; keep `UseAntiforgery` enabled in `Program.cs`.
- Show failures to the user (message or error boundary); do not let exceptions end the circuit silently.
- Keep markup readable: extract a child component when a block repeats or exceeds roughly 50 lines.
- Move `@code` blocks to `.razor.cs` once they exceed roughly 30 lines.
