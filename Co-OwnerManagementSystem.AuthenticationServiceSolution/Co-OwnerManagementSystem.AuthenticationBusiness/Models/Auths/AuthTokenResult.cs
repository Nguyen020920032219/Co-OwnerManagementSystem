namespace Co_OwnerManagementSystem.AuthenticationBusiness.Models.Auths;

public class AuthTokenResult
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
    public DateTime AccessTokenExpiresAt { get; set; }
}