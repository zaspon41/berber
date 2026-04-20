namespace Application.Interfaces;

using System.Threading.Tasks;
using Domain.Entities;

public interface IAdminAuthRepository
{
    Task<Admin?> GetByCredentialsAsync(string adminUserName, string adminPassword);
}