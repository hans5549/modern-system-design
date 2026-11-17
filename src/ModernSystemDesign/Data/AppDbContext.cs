using Microsoft.EntityFrameworkCore;
using ModernSystemDesign.Models;

namespace ModernSystemDesign.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Note> Notes => Set<Note>();
}
