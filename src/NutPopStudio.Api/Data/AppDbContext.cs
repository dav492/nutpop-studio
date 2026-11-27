using Microsoft.EntityFrameworkCore;
using NutPopStudio.Api.Models;

namespace NutPopStudio.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Measurement> Measurements => Set<Measurement>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Measurement>()
            .HasOne(m => m.Patient)
            .WithMany(p => p.Measurements)
            .HasForeignKey(m => m.PatientId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId);

        modelBuilder.Entity<Appointment>()
            .Property(a => a.Status)
            .HasConversion<string>();
    }
}