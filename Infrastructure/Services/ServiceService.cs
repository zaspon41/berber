namespace Infrastructure.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IValidator<CreateServiceRequest> _createValidator;
    private readonly IValidator<UpdateServiceRequest> _updateValidator;
    private readonly IMapper _mapper;

    public ServiceService(
        IServiceRepository serviceRepository,
        IValidator<CreateServiceRequest> createValidator,
        IValidator<UpdateServiceRequest> updateValidator,
        IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _mapper = mapper;
    }

    public async Task<ServiceResponse> CreateServiceAsync(CreateServiceRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new Application.Exceptions.ValidationException(errors);
        }

        var hizmet = _mapper.Map<Hizmet>(request);
        var createdHizmet = await _serviceRepository.CreateAsync(hizmet);

        return _mapper.Map<ServiceResponse>(createdHizmet);
    }

    public async Task<ServiceResponse> GetServiceByIdAsync(int id)
    {
        if (id <= 0)
            throw new BadRequestException("Geçerli bir hizmet ID'si gereklidir");

        var hizmet = await _serviceRepository.GetByIdAsync(id);
        if (hizmet == null)
            throw new NotFoundException($"ID {id} ile hizmet bulunamadı");

        return _mapper.Map<ServiceResponse>(hizmet);
    }

    public async Task<List<ServiceResponse>> GetAllServicesAsync()
    {
        var hizmetler = await _serviceRepository.GetAllAsync();
        return _mapper.Map<List<ServiceResponse>>(hizmetler);
    }

    public async Task<ServiceResponse> UpdateServiceAsync(UpdateServiceRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new Application.Exceptions.ValidationException(errors);
        }

        var exists = await _serviceRepository.ExistsByIdAsync(request.Id);
        if (!exists)
            throw new NotFoundException($"ID {request.Id} ile hizmet bulunamadı");

        var hizmet = _mapper.Map<Hizmet>(request);
        var updatedHizmet = await _serviceRepository.UpdateAsync(hizmet);

        return _mapper.Map<ServiceResponse>(updatedHizmet);
    }

    public async Task<bool> DeleteServiceAsync(int id)
    {
        if (id <= 0)
            throw new BadRequestException("Geçerli bir hizmet ID'si gereklidir");

        var exists = await _serviceRepository.ExistsByIdAsync(id);
        if (!exists)
            throw new NotFoundException($"ID {id} ile hizmet bulunamadı");

        return await _serviceRepository.DeleteAsync(id);
    }
}
