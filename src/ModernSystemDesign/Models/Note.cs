namespace ModernSystemDesign.Models;

// init properties (not positional) so both EF Core and Dapper can materialize it; SQLite returns INTEGER as long, which breaks Dapper's int ctor matching.
public record Note
{
    public int Id { get; init; }
    public string Title { get; init; } = "";
}
