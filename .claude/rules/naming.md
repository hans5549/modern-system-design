# Naming

Basis: Microsoft Framework Design Guidelines (naming) and C# identifier-name conventions.
The `.editorconfig` naming rules (IDE1006) enforce the code rows marked with an asterisk.

## Code

| Item | Convention | Example |
|---|---|---|
| Namespace | PascalCase, matches folder path | `ModernSystemDesign.Services` |
| Class, record, struct, enum* | PascalCase, noun | `NoteService` |
| Interface* | `I` + PascalCase | `INoteRepository` |
| Method, property, event* | PascalCase | `GetAllAsync`, `CreatedAt` |
| Async method | `Async` suffix on every method that returns `Task` / `ValueTask` (tooling only checks methods declared `async`) | `AddAsync` |
| Private field* | `_camelCase` | `_notes` |
| Primary-constructor parameter | camelCase | `NoteService(INoteRepository repository)` |
| Parameter, local variable | camelCase | `noteId` |
| Constant | PascalCase | `MaxTitleLength` |
| Boolean | Prefix `Is`, `Has`, `Can`, `Should` | `IsCompleted` |
| Generic type parameter | `T` or `T` + PascalCase | `TEntity` |

- Services are `<Entity>Service`; repositories are `<Entity>Repository`; the DbContext is `AppDbContext`.
- `DbSet` properties and table names are plural PascalCase (`Notes`).
- Use full words, no abbreviations (except well-known ones such as `Id`, `Url`). No Hungarian notation.
- Do not repeat the type in the member name (`Note.Title`, not `Note.NoteTitle`).

## Razor / Blazor

- Component files: PascalCase (`NoteList.razor`). Routable pages live in `Components/Pages/`.
- Non-trivial `@code` logic moves to a code-behind partial class `NoteList.razor.cs`.
- Scoped styles: `NoteList.razor.css`.
- Component parameters: PascalCase with `[Parameter]`. `EventCallback` parameters start with `On` (`OnSaved`).
- Handler methods in a component: `HandleXxxAsync` (`HandleAddAsync`).

## Data, config, logging

- Dapper SQL: keywords UPPERCASE; table and column names PascalCase, matching the EF model.
- Configuration keys: PascalCase (`ConnectionStrings:Default`).
- Log message template placeholders: PascalCase (`{NoteId}`).

## Tests

- Test project: `ModernSystemDesign.Tests`. Test class: `<ClassUnderTest>Tests` (`NoteServiceTests`).
- Test method: `MethodName_Scenario_ExpectedResult` (`AddAsync_BlankTitle_Throws`).

## Git

- Branches: `feature/<topic>`, `fix/<topic>`, `docs/<topic>`, `chore/<topic>` (lowercase, hyphen-separated).
- Commits: Conventional Commits, for example `feat: add note deletion`.
