namespace Application.DTOs;

using System;

public class CreateBlockedDatesRequest
{
    public DateOnly Tarih { get; set; }
    public string? Neden { get; set; }
}

public class UpdateBlockedDatesRequest
{
    public int Id { get; set; }
    public DateOnly Tarih { get; set; }
    public string? Neden { get; set; }
}

public class BlockedDatesResponse
{
    public int Id { get; set; }
    public DateOnly Tarih { get; set; }
    public string? Neden { get; set; }
    public DateTime OluşturulmaTarihi { get; set; }
}
