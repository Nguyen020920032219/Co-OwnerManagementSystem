using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Co_OwnerManagementSystem.AuthenticationData.Entities;

[Table("UserProfile")]
[Index("UserId", Name = "UQ__UserProf__1788CC4DB807D455", IsUnique = true)]
public partial class UserProfile
{
    [Key]
    public int ProfileId { get; set; }

    [StringLength(255)]
    public string? Email { get; set; }

    [StringLength(255)]
    public string FirstName { get; set; } = null!;

    [StringLength(255)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserProfile")]
    public virtual AppUser User { get; set; } = null!;
}
