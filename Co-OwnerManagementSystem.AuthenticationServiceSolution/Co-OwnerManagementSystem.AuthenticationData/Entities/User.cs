using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Co_OwnerManagementSystem.AuthenticationData.Entities;

[Table("User")]
[Index("PhoneNumber", Name = "UQ__User__85FB4E3886FDEC7A", IsUnique = true)]
public partial class User
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

    [InverseProperty("User")]
    public virtual Profile? Profile { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;
}
