using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

public class OfficesDbContext : DbContext
{
    public OfficesDbContext(DbContextOptions<OfficesDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Office>().ToCollection("Offices");
    }
    public DbSet<Office> Offices { get; set; }
}