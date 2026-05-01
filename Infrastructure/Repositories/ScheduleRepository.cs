namespace Infrastructure.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class OperatingHoursRepository : IOperatingHoursRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OperatingHoursRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperatingHours?> GetByIdAsync(int id)
    {
        return await _dbContext.OperatingHours
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<OperatingHours?> GetByDayAsync(int dayOfWeek)
    {
        return await _dbContext.OperatingHours
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.DayOfWeek == dayOfWeek);
    }

    public async Task<List<OperatingHours>> GetAllAsync()
    {
        return await _dbContext.OperatingHours
            .AsNoTracking()
            .OrderBy(o => o.DayOfWeek)
            .ToListAsync();
    }

    public async Task<OperatingHours> CreateAsync(OperatingHours operatingHours)
    {
        _dbContext.OperatingHours.Add(operatingHours);
        await _dbContext.SaveChangesAsync();
        return operatingHours;
    }

    public async Task<OperatingHours> UpdateAsync(OperatingHours operatingHours)
    {
        _dbContext.OperatingHours.Update(operatingHours);
        await _dbContext.SaveChangesAsync();
        return operatingHours;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var operatingHours = await _dbContext.OperatingHours
            .FirstOrDefaultAsync(o => o.Id == id);
        
        if (operatingHours == null)
            return false;

        _dbContext.OperatingHours.Remove(operatingHours);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}

public class BlockedDatesRepository : IBlockedDatesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BlockedDatesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BlockedDates?> GetByIdAsync(int id)
    {
        return await _dbContext.BlockedDates
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<BlockedDates>> GetAllAsync()
    {
        return await _dbContext.BlockedDates
            .AsNoTracking()
            .OrderBy(b => b.Tarih)
            .ToListAsync();
    }

    public async Task<BlockedDates?> GetByDateAsync(DateTime date)
    {
        var dateOnly = DateOnly.FromDateTime(date);
        return await _dbContext.BlockedDates
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Tarih == dateOnly);
    }

    public async Task<List<BlockedDates>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate)
    {
        return await _dbContext.BlockedDates
            .AsNoTracking()
            .Where(b => b.Tarih >= startDate && b.Tarih <= endDate)
            .OrderBy(b => b.Tarih)
            .ToListAsync();
    }

    public async Task<BlockedDates> CreateAsync(BlockedDates blockedDates)
    {
        _dbContext.BlockedDates.Add(blockedDates);
        await _dbContext.SaveChangesAsync();
        return blockedDates;
    }

    public async Task<BlockedDates> UpdateAsync(BlockedDates blockedDates)
    {
        _dbContext.BlockedDates.Update(blockedDates);
        await _dbContext.SaveChangesAsync();
        return blockedDates;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var blockedDates = await _dbContext.BlockedDates
            .FirstOrDefaultAsync(b => b.Id == id);
        
        if (blockedDates == null)
            return false;

        _dbContext.BlockedDates.Remove(blockedDates);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AppointmentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await _dbContext.Appointment
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Appointment>> GetAllAsync()
    {
        return await _dbContext.Appointment
            .AsNoTracking()
            .OrderByDescending(a => a.RandevuTarihi)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetByDateAsync(DateTime date)
    {
        return await _dbContext.Appointment
            .AsNoTracking()
            .Where(a => a.RandevuTarihi.Date == date.Date)
            .OrderBy(a => a.RandevuSaati)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbContext.Appointment
            .AsNoTracking()
            .Where(a => a.RandevuTarihi.Date >= startDate.Date && a.RandevuTarihi.Date <= endDate.Date)
            .OrderBy(a => a.RandevuTarihi)
            .ThenBy(a => a.RandevuSaati)
            .ToListAsync();
    }

    public async Task<Appointment?> CheckAvailabilityAsync(DateTime date, TimeSpan time)
    {
        return await _dbContext.Appointment
            .AsNoTracking()
            .FirstOrDefaultAsync(a => 
                a.RandevuTarihi.Date == date.Date && 
                a.RandevuSaati == time &&
                a.Durum != "İptal");
    }

    public async Task<Appointment> CreateAsync(Appointment appointment)
    {
        _dbContext.Appointment.Add(appointment);
        await _dbContext.SaveChangesAsync();
        return appointment;
    }

    public async Task<Appointment> UpdateAsync(Appointment appointment)
    {
        _dbContext.Appointment.Update(appointment);
        await _dbContext.SaveChangesAsync();
        return appointment;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var appointment = await _dbContext.Appointment
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if (appointment == null)
            return false;

        _dbContext.Appointment.Remove(appointment);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
