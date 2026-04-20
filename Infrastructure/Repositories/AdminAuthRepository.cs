namespace Infrastructure.Repositories;

using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class AdminAuthRepository : IAdminAuthRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AdminAuthRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Admin?> GetByCredentialsAsync(string adminUserName, string adminPassword)
    {
        return await _dbContext.Admins
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AdminUserName == adminUserName && a.AdminPassword == adminPassword);
    }
}