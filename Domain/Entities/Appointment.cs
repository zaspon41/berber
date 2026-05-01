namespace Domain.Entities;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Appointment")]
public class Appointment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("müşteriAdı")]
    public string MüşteriAdı { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    [Column("müşteriTelefon")]
    public string MüşteriTelefon { get; set; } = null!;

    [Required]
    [Column("hizmetId")]
    public int HizmetId { get; set; }

    [Required]
    [Column("randevuTarihi")]
    public DateTime RandevuTarihi { get; set; }

    [Required]
    [Column("randevuSaati")]
    public TimeSpan RandevuSaati { get; set; }

    [Column("durum")]
    [MaxLength(20)]
    public string Durum { get; set; } = "Beklemede";

    [Column("notlar")]
    [MaxLength(500)]
    public string? Notlar { get; set; }

    [Column("oluşturulmaTarihi")]
    public DateTime OluşturulmaTarihi { get; set; } = DateTime.Now;
}
