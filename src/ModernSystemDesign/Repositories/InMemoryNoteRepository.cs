using ModernSystemDesign.Models;

namespace ModernSystemDesign.Repositories;

// shortcut: in-memory seed data, swap for a DB-backed implementation (e.g. EF Core) when persistence is needed.
public class InMemoryNoteRepository : INoteRepository
{
    private static readonly IReadOnlyList<Note> Notes =
    [
        new(1, "Hello from the repository layer"),
        new(2, "Services call repositories, pages call services"),
    ];

    public Task<IReadOnlyList<Note>> GetAllAsync() => Task.FromResult(Notes);
}
