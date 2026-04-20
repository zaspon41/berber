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
}
