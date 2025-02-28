using Microsoft.EntityFrameworkCore;

public class AppointmentsDbContext : DbContext
{
    public AppointmentsDbContext(DbContextOptions<AppointmentsDbContext> options) : base(options) 
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>()
           .Property(e => e.Date)
           .HasConversion(
               v => v.ToUniversalTime(),
               v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
           );

        modelBuilder.Entity<Results>()
            .Property(e => e.Date)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
    }

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Results> Results { get; set; }
    public DbSet<Service> Services { get; set; }
}
