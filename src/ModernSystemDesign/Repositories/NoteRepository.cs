using Dapper;
using Microsoft.EntityFrameworkCore;
using ModernSystemDesign.Data;
using ModernSystemDesign.Models;

namespace ModernSystemDesign.Repositories;

// Reads go through Dapper, writes through EF Core; both share the context's connection (same connection string).
// A short-lived context per call (factory) is the Blazor Server pattern: a circuit-lifetime DbContext goes stale.
// shortcut: Dapper runs outside any EF transaction, pass db.Database.CurrentTransaction's DbTransaction if reads must join a write.
public class NoteRepository(IDbContextFactory<AppDbContext> factory) : INoteRepository
{
    public async Task<IReadOnlyList<Note>> GetAllAsync()
    {
        await using var db = await factory.CreateDbContextAsync();
        var notes = await db.Database.GetDbConnection().QueryAsync<Note>("SELECT Id, Title FROM Notes ORDER BY Id");
        return notes.AsList();
    }

    public async Task AddAsync(Note note)
    {
        await using var db = await factory.CreateDbContextAsync();
        db.Notes.Add(note);
        await db.SaveChangesAsync();
    }
}
