namespace Infrastructure.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class ServiceRepository : IServiceRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ServiceRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Hizmet?> GetByIdAsync(int id)
    {
        return await _dbContext.Hizmetler
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<List<Hizmet>> GetAllAsync()
    {
        return await _dbContext.Hizmetler
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Hizmet> CreateAsync(Hizmet hizmet)
    {
        _dbContext.Hizmetler.Add(hizmet);
        await _dbContext.SaveChangesAsync();
        return hizmet;
    }

    public async Task<Hizmet> UpdateAsync(Hizmet hizmet)
    {
        _dbContext.Hizmetler.Update(hizmet);
        await _dbContext.SaveChangesAsync();
        return hizmet;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var hizmet = await _dbContext.Hizmetler.FirstOrDefaultAsync(h => h.Id == id);
        if (hizmet == null)
            return false;

        _dbContext.Hizmetler.Remove(hizmet);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await _dbContext.Hizmetler.AnyAsync(h => h.Id == id);
    }
}
