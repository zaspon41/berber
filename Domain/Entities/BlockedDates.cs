namespace Domain.Entities;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("BlockedDates")]
public class BlockedDates
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("tarih")]
    public DateOnly Tarih { get; set; }

    [Column("neden")]
    [MaxLength(200)]
    public string? Neden { get; set; }

    [Column("oluşturulmaTarihi")]
    public DateTime OluşturulmaTarihi { get; set; } = DateTime.Now;
}
