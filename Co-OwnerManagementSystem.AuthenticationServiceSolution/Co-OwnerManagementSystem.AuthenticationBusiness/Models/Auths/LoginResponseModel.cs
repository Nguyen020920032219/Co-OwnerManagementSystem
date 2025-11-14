namespace Co_OwnerManagementSystem.AuthenticationBusiness.Models.Auths
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
    }
}
