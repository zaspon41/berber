namespace Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

public interface IOperatingHoursRepository
{
    Task<OperatingHours?> GetByIdAsync(int id);
    Task<OperatingHours?> GetByDayAsync(int dayOfWeek);
    Task<List<OperatingHours>> GetAllAsync();
    Task<OperatingHours> CreateAsync(OperatingHours operatingHours);
    Task<OperatingHours> UpdateAsync(OperatingHours operatingHours);
    Task<bool> DeleteAsync(int id);
}

public interface IBlockedDatesRepository
{
    Task<BlockedDates?> GetByIdAsync(int id);
    Task<List<BlockedDates>> GetAllAsync();
    Task<BlockedDates?> GetByDateAsync(DateTime date);
    Task<List<BlockedDates>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate);
    Task<BlockedDates> CreateAsync(BlockedDates blockedDates);
    Task<BlockedDates> UpdateAsync(BlockedDates blockedDates);
    Task<bool> DeleteAsync(int id);
}

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(int id);
    Task<List<Appointment>> GetAllAsync();
    Task<List<Appointment>> GetByDateAsync(DateTime date);
    Task<List<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Appointment?> CheckAvailabilityAsync(DateTime date, TimeSpan time);
    Task<Appointment> CreateAsync(Appointment appointment);
    Task<Appointment> UpdateAsync(Appointment appointment);
    Task<bool> DeleteAsync(int id);
}
