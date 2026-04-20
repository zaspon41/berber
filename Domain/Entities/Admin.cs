namespace Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Admin")]
public class Admin
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("adminUserName")]
    public string AdminUserName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    [Column("adminPassword")]
    public string AdminPassword { get; set; } = null!;
}
