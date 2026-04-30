namespace Application.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

public interface IServiceRepository
{
    Task<Hizmet?> GetByIdAsync(int id);
    Task<List<Hizmet>> GetAllAsync();
    Task<Hizmet> CreateAsync(Hizmet hizmet);
    Task<Hizmet> UpdateAsync(Hizmet hizmet);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByIdAsync(int id);
}
