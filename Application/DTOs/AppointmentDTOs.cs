namespace Application.DTOs;

using System;

public class CreateAppointmentRequest
{
    public string MüşteriAdı { get; set; } = null!;
    public string MüşteriTelefon { get; set; } = null!;
    public int HizmetId { get; set; }
    public DateTime RandevuTarihi { get; set; }
    public TimeSpan RandevuSaati { get; set; }
    public string? Notlar { get; set; }
}

public class UpdateAppointmentRequest
{
    public int Id { get; set; }
    public string MüşteriAdı { get; set; } = null!;
    public string MüşteriTelefon { get; set; } = null!;
    public int HizmetId { get; set; }
    public DateTime RandevuTarihi { get; set; }
    public TimeSpan RandevuSaati { get; set; }
    public string Durum { get; set; } = null!;
    public string? Notlar { get; set; }
}

public class AppointmentResponse
{
    public int Id { get; set; }
    public string MüşteriAdı { get; set; } = null!;
    public string MüşteriTelefon { get; set; } = null!;
    public int HizmetId { get; set; }
    public DateTime RandevuTarihi { get; set; }
    public TimeSpan RandevuSaati { get; set; }
    public string Durum { get; set; } = null!;
    public string? Notlar { get; set; }
    public DateTime OluşturulmaTarihi { get; set; }
}
