using Microsoft.EntityFrameworkCore;

public class DocumentsDbContext : DbContext
{
    public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Photo> Photos { get; set; }
}