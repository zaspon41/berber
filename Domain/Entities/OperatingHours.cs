namespace Domain.Entities;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("OperatingHours")]
public class OperatingHours
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("dayOfWeek")]
    public int DayOfWeek { get; set; } // 0=Pazar, 1=Pazartesi, ..., 6=Cumartesi

    [Required]
    [Column("açılışSaati")]
    public TimeSpan AçılışSaati { get; set; }

    [Required]
    [Column("kapanışSaati")]
    public TimeSpan KapanışSaati { get; set; }

    [Required]
    [Column("açıkMı")]
    public bool AçıkMı { get; set; } = true;
}
