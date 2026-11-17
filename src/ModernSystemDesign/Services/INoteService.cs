using ModernSystemDesign.Models;

namespace ModernSystemDesign.Services;

public interface INoteService
{
    Task<IReadOnlyList<Note>> GetAllAsync();
}
