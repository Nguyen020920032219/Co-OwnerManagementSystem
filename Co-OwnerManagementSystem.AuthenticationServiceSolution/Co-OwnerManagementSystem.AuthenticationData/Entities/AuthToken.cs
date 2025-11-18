using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Co_OwnerManagementSystem.AuthenticationData.Entities;

[Table("AuthToken")]
public partial class AuthToken
{
    [Key]
    public int Id { get; set; }

    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public bool? Valid { get; set; }

    public DateTimeOffset ExpiredAt { get; set; }

    public int? UserId { get; set; }

    public DateTimeOffset RefreshTokenExpiredAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("AuthTokens")]
    public virtual AppUser? User { get; set; }
}
