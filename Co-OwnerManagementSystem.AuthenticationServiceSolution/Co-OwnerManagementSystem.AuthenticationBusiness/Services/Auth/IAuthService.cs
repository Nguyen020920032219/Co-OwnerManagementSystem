using System.Security.Claims;
using Co_OwnerManagementSystem.AuthenticationBusiness.Models;
using Co_OwnerManagementSystem.AuthenticationBusiness.Models.Auths;
using Co_OwnerManagementSystem.AuthenticationBusiness.Models.Users;

namespace Co_OwnerManagementSystem.AuthenticationBusiness.Services.Auth;

public interface IAuthService
{
    Task<LoginResponseModel> Login(LoginRequestModel request);
    Task<UserModel> Register(RegisterUserRequestModel request);
    Task<UserModel> GetMyProfile(int userId);
    Task<bool> UpdateProfile(int userId, UpdateProfileModel model);
}