using System.Security.Claims;
using Co_OwnerManagementSystem.AuthenticationBusiness.Models.Auths;
using Co_OwnerManagementSystem.AuthenticationData.Entities;
using Microsoft.AspNetCore.Http;

namespace Co_OwnerManagementSystem.AuthenticationBusiness.Services.Auth;

public interface ITokenService
{
    string GenerateAccessToken(List<Claim> clams, DateTime tokenExpireAt);
    string GenerateRefreshToken();
    DateTime GenerateAccessTokenExpiration();
    DateTime GenerateRefreshTokenExpiration();
    string ComputeSha256(string input);
    
    Task InvalidateToken(string refreshToken);
    Task<AuthTokenResult> CreateAuthToken(User user);
}