namespace Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Hizmetler")]
public class Hizmet
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("hizmet")]
    public string HizmetAdi { get; set; } = null!;

    [Required]
    [Column("fiyat")]
    public int Fiyat { get; set; }
}
