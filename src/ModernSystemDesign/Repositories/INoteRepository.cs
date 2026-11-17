using ModernSystemDesign.Models;

namespace ModernSystemDesign.Repositories;

public interface INoteRepository
{
    Task<IReadOnlyList<Note>> GetAllAsync();
    Task AddAsync(Note note);
}
