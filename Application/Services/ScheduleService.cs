namespace Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;

public class OperatingHoursService : IOperatingHoursService
{
    private readonly IOperatingHoursRepository _repository;
    private readonly IValidator<CreateOperatingHoursRequest> _createValidator;
    private readonly IValidator<UpdateOperatingHoursRequest> _updateValidator;
    private readonly IMapper _mapper;

    public OperatingHoursService(
        IOperatingHoursRepository repository,
        IValidator<CreateOperatingHoursRequest> createValidator,
        IValidator<UpdateOperatingHoursRequest> updateValidator,
        IMapper mapper)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _mapper = mapper;
    }

    public async Task<OperatingHoursResponse> CreateAsync(CreateOperatingHoursRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new Application.Exceptions.ValidationException(errors);
        }

        var operatingHours = _mapper.Map<OperatingHours>(request);
        var created = await _repository.CreateAsync(operatingHours);
        return _mapper.Map<OperatingHoursResponse>(created);
    }

    public async Task<OperatingHoursResponse?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new BadRequestException("Geçerli ID gereklidir");

        var operatingHours = await _repository.GetByIdAsync(id);
        if (operatingHours == null)
            throw new NotFoundException($"ID {id} ile çalışma saati bulunamadı");

        return _mapper.Map<OperatingHoursResponse>(operatingHours);
    }

    public async Task<OperatingHoursResponse?> GetByDayAsync(int dayOfWeek)
    {
        if (dayOfWeek < 0 || dayOfWeek > 6)
            throw new BadRequestException("Gün 0-6 arasında olmalıdır");

        var operatingHours = await _repository.GetByDayAsync(dayOfWeek);
        if (operatingHours == null)
            return null;

        return _mapper.Map<OperatingHoursResponse>(operatingHours);
    }

    public async Task<List<OperatingHoursResponse>> GetAllAsync()
    {
        var operatingHours = await _repository.GetAllAsync();
        return _mapper.Map<List<OperatingHoursResponse>>(operatingHours);
    }

    public async Task<OperatingHoursResponse> UpdateAsync(UpdateOperatingHoursRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new Application.Exceptions.ValidationException(errors);
        }

        var existing = await _repository.GetByIdAsync(request.Id);
        if (existing == null)
            throw new NotFoundException($"ID {request.Id} ile çalışma saati bulunamadı");

        var operatingHours = _mapper.Map<OperatingHours>(request);
        var updated = await _repository.UpdateAsync(operatingHours);
        return _mapper.Map<OperatingHoursResponse>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new BadRequestException("Geçerli ID gereklidir");

        var exists = await _repository.GetByIdAsync(id);
        if (exists == null)
            throw new NotFoundException($"ID {id} ile çalışma saati bulunamadı");

        return await _repository.DeleteAsync(id);
    }
}

public class BlockedDatesService : IBlockedDatesService
{
    private readonly IBlockedDatesRepository _repository;
    private readonly IValidator<CreateBlockedDatesRequest> _createValidator;
    private readonly IValidator<UpdateBlockedDatesRequest> _updateValidator;
    private readonly IMapper _mapper;

    public BlockedDatesService(
        IBlockedDatesRepository repository,
        IValidator<CreateBlockedDatesRequest> createValidator,
        IValidator<UpdateBlockedDatesRequest> updateValidator,
        IMapper mapper)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _mapper = mapper;
    }

    public async Task<BlockedDatesResponse> CreateAsync(CreateBlockedDatesRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new Application.Exceptions.ValidationException(errors);
        }

        var blockedDates = _mapper.Map<BlockedDates>(request);
        var created = await _repository.CreateAsync(blockedDates);
        return _mapper.Map<BlockedDatesResponse>(created);
    }

    public async Task<BlockedDatesResponse?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new BadRequestException("Geçerli ID gereklidir");

        var blockedDates = await _repository.GetByIdAsync(id);
        if (blockedDates == null)
            throw new NotFoundException($"ID {id} ile kapalı gün bulunamadı");

        return _mapper.Map<BlockedDatesResponse>(blockedDates);
    }

    public async Task<List<BlockedDatesResponse>> GetAllAsync()
    {
        var blockedDates = await _repository.GetAllAsync();
        return _mapper.Map<List<BlockedDatesResponse>>(blockedDates);
    }

    public async Task<List<BlockedDatesResponse>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate)
    {
        if (startDate > endDate)
            throw new BadRequestException("Başlangıç tarihi bitiş tarihinden önce olmalıdır");

        var blockedDates = await _repository.GetByDateRangeAsync(startDate, endDate);
        return _mapper.Map<List<BlockedDatesResponse>>(blockedDates);
    }

    public async Task<BlockedDatesResponse> UpdateAsync(UpdateBlockedDatesRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new Application.Exceptions.ValidationException(errors);
        }

        var existing = await _repository.GetByIdAsync(request.Id);
        if (existing == null)
            throw new NotFoundException($"ID {request.Id} ile kapalı gün bulunamadı");

        var blockedDates = _mapper.Map<BlockedDates>(request);
        var updated = await _repository.UpdateAsync(blockedDates);
        return _mapper.Map<BlockedDatesResponse>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new BadRequestException("Geçerli ID gereklidir");

        var exists = await _repository.GetByIdAsync(id);
        if (exists == null)
            throw new NotFoundException($"ID {id} ile kapalı gün bulunamadı");

        return await _repository.DeleteAsync(id);
    }
}

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repository;
    private readonly IValidator<CreateAppointmentRequest> _createValidator;
    private readonly IValidator<UpdateAppointmentRequest> _updateValidator;
    private readonly IMapper _mapper;

    public AppointmentService(
        IAppointmentRepository repository,
        IValidator<CreateAppointmentRequest> createValidator,
        IValidator<UpdateAppointmentRequest> updateValidator,
        IMapper mapper)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _mapper = mapper;
    }

    public async Task<AppointmentResponse> CreateAsync(CreateAppointmentRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new Application.Exceptions.ValidationException(errors);
        }

        var existing = await _repository.CheckAvailabilityAsync(request.RandevuTarihi, request.RandevuSaati);
        if (existing != null)
            throw new BadRequestException("Bu saatte zaten bir randevu var");

        var appointment = _mapper.Map<Appointment>(request);
        var created = await _repository.CreateAsync(appointment);
        return _mapper.Map<AppointmentResponse>(created);
    }

    public async Task<AppointmentResponse?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new BadRequestException("Geçerli ID gereklidir");

        var appointment = await _repository.GetByIdAsync(id);
        if (appointment == null)
            throw new NotFoundException($"ID {id} ile randevu bulunamadı");

        return _mapper.Map<AppointmentResponse>(appointment);
    }

    public async Task<List<AppointmentResponse>> GetAllAsync()
    {
        var appointments = await _repository.GetAllAsync();
        return _mapper.Map<List<AppointmentResponse>>(appointments);
    }

    public async Task<List<AppointmentResponse>> GetByDateAsync(DateTime date)
    {
        var appointments = await _repository.GetByDateAsync(date);
        return _mapper.Map<List<AppointmentResponse>>(appointments);
    }

    public async Task<List<AppointmentResponse>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new BadRequestException("Başlangıç tarihi bitiş tarihinden önce olmalıdır");

        var appointments = await _repository.GetByDateRangeAsync(startDate, endDate);
        return _mapper.Map<List<AppointmentResponse>>(appointments);
    }

    public async Task<AppointmentResponse> UpdateAsync(UpdateAppointmentRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new Application.Exceptions.ValidationException(errors);
        }

        var existing = await _repository.GetByIdAsync(request.Id);
        if (existing == null)
            throw new NotFoundException($"ID {request.Id} ile randevu bulunamadı");

        var appointment = _mapper.Map<Appointment>(request);
        var updated = await _repository.UpdateAsync(appointment);
        return _mapper.Map<AppointmentResponse>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new BadRequestException("Geçerli ID gereklidir");

        var exists = await _repository.GetByIdAsync(id);
        if (exists == null)
            throw new NotFoundException($"ID {id} ile randevu bulunamadı");

        return await _repository.DeleteAsync(id);
    }
}
