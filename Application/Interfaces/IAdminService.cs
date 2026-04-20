namespace Application.Interfaces;

using System.Threading.Tasks;
using Domain.Entities;
using Application.DTOs;

public interface IAdminService
{
    Task<AdminLoginResponse> LoginAsync(AdminLoginRequest request);
}
