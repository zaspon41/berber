namespace Application.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;

public interface IServiceService
{
    Task<ServiceResponse> CreateServiceAsync(CreateServiceRequest request);
    Task<ServiceResponse> GetServiceByIdAsync(int id);
    Task<List<ServiceResponse>> GetAllServicesAsync();
    Task<ServiceResponse> UpdateServiceAsync(UpdateServiceRequest request);
    Task<bool> DeleteServiceAsync(int id);
}
