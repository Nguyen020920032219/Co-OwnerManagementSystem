namespace Co_OwnerManagementSystem.AuthenticationBusiness.Models.Users;

public class UserModel
{
    public int UserId { get; set; }
    
    public string PhoneNumber { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
    
    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    // [StringLength(255)]
    // public string? CitizenIdentification { get; set; } = null!;
    //
    // [StringLength(255)]
    // public string? DrivingLicense { get; set; } = null!;
}