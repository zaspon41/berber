namespace Application.DTOs;

using System;

public class CreateOperatingHoursRequest
{
    public int DayOfWeek { get; set; }
    public TimeSpan AçılışSaati { get; set; }
    public TimeSpan KapanışSaati { get; set; }
    public bool AçıkMı { get; set; } = true;
}

public class UpdateOperatingHoursRequest
{
    public int Id { get; set; }
    public int DayOfWeek { get; set; }
    public TimeSpan AçılışSaati { get; set; }
    public TimeSpan KapanışSaati { get; set; }
    public bool AçıkMı { get; set; }
}

public class OperatingHoursResponse
{
    public int Id { get; set; }
    public int DayOfWeek { get; set; }
    public TimeSpan AçılışSaati { get; set; }
    public TimeSpan KapanışSaati { get; set; }
    public bool AçıkMı { get; set; }
}
