namespace Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;

/// <summary>
/// Database context for Berber Randevu application
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<Hizmet> Hizmetler { get; set; }
    public DbSet<OperatingHours> OperatingHours { get; set; }
    public DbSet<BlockedDates> BlockedDates { get; set; }
    public DbSet<Appointment> Appointment { get; set; }
}
