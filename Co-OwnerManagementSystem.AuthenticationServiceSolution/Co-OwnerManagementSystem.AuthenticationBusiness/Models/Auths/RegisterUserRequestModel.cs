using System.ComponentModel.DataAnnotations;

namespace Co_OwnerManagementSystem.AuthenticationBusiness.Models.Auths;

public class RegisterUserRequestModel
{
    [Required]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string Password { get; set; } = null!;
    
    public string PasswordConfirm { get; set; } = null!;
    
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [StringLength(255)]
    public string LastName { get; set; } = null!;

    [RegularExpression("^(?i)(male|female|other)$", ErrorMessage = "gender must be male, female or other")]
    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }
    
}