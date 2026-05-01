namespace Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;

public interface IOperatingHoursService
{
    Task<OperatingHoursResponse> CreateAsync(CreateOperatingHoursRequest request);
    Task<OperatingHoursResponse?> GetByIdAsync(int id);
    Task<OperatingHoursResponse?> GetByDayAsync(int dayOfWeek);
    Task<List<OperatingHoursResponse>> GetAllAsync();
    Task<OperatingHoursResponse> UpdateAsync(UpdateOperatingHoursRequest request);
    Task<bool> DeleteAsync(int id);
}

public interface IBlockedDatesService
{
    Task<BlockedDatesResponse> CreateAsync(CreateBlockedDatesRequest request);
    Task<BlockedDatesResponse?> GetByIdAsync(int id);
    Task<List<BlockedDatesResponse>> GetAllAsync();
    Task<List<BlockedDatesResponse>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate);
    Task<BlockedDatesResponse> UpdateAsync(UpdateBlockedDatesRequest request);
    Task<bool> DeleteAsync(int id);
}

public interface IAppointmentService
{
    Task<AppointmentResponse> CreateAsync(CreateAppointmentRequest request);
    Task<AppointmentResponse?> GetByIdAsync(int id);
    Task<List<AppointmentResponse>> GetAllAsync();
    Task<List<AppointmentResponse>> GetByDateAsync(DateTime date);
    Task<List<AppointmentResponse>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<AppointmentResponse> UpdateAsync(UpdateAppointmentRequest request);
    Task<bool> DeleteAsync(int id);
}
