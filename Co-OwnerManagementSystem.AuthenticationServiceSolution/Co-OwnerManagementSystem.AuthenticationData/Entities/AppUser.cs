using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Co_OwnerManagementSystem.AuthenticationData.Entities;

[Table("AppUser")]
[Index("PhoneNumber", Name = "UQ__AppUser__85FB4E3854560FCC", IsUnique = true)]
public partial class AppUser
{
    [Key]
    public int UserId { get; set; }

    [StringLength(255)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<AuthToken> AuthTokens { get; set; } = new List<AuthToken>();

    [ForeignKey("RoleId")]
    [InverseProperty("AppUsers")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("User")]
    public virtual UserProfile? UserProfile { get; set; }
}
