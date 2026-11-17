using ModernSystemDesign.Models;
using ModernSystemDesign.Repositories;

namespace ModernSystemDesign.Services;

public class NoteService(INoteRepository repository) : INoteService
{
    public Task<IReadOnlyList<Note>> GetAllAsync() => repository.GetAllAsync();

    public Task AddAsync(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        return repository.AddAsync(new Note { Title = title.Trim() });
    }
}
