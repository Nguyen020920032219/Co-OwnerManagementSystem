using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Co_OwnerManagementSystem.AuthenticationData.Entities;

[Table("Profile")]
[Index("UserId", Name = "UQ__Profile__1788CC4D1757315A", IsUnique = true)]
[Index("CitizenIdentification", Name = "UQ__Profile__3C297E7763617A35", IsUnique = true)]
[Index("DrivingLicense", Name = "UQ__Profile__5F5D2739DEB4D569", IsUnique = true)]
[Index("Email", Name = "UQ__Profile__A9D105349907DDB2", IsUnique = true)]
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
