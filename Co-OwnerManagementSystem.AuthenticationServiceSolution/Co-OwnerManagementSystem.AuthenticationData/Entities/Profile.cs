using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Co_OwnerManagementSystem.AuthenticationData.Entities;

[Table("Profile")]
[Index("UserId", Name = "UQ__Profile__1788CC4D8B439725", IsUnique = true)]
[Index("CitizenIdentification", Name = "UQ__Profile__3C297E77F77ABA68", IsUnique = true)]
[Index("DrivingLicense", Name = "UQ__Profile__5F5D273932D028F2", IsUnique = true)]
[Index("Email", Name = "UQ__Profile__A9D10534A74CB842", IsUnique = true)]
public partial class Profile
{
    [Key]
    public int ProfileId { get; set; }

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string FirstName { get; set; } = null!;

    [StringLength(255)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    [StringLength(255)]
    public string CitizenIdentification { get; set; } = null!;

    [StringLength(255)]
    public string DrivingLicense { get; set; } = null!;

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Profile")]
    public virtual User User { get; set; } = null!;
}
