using Microsoft.EntityFrameworkCore;

public class ProfilesDbContext : DbContext
{
    public ProfilesDbContext(DbContextOptions<ProfilesDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>()
            .Property(e => e.DateOfBirth)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc) 
            );

        modelBuilder.Entity<Patient>()
            .Property(e => e.DateOfBirth)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Receptionist> Receptionists { get; set; }
    public DbSet<Office> Office { get; set; }
}
