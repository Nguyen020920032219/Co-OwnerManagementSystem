using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Co_OwnerManagementSystem.AuthenticationBusiness.Configurations;
using Co_OwnerManagementSystem.AuthenticationBusiness.Constants;
using Co_OwnerManagementSystem.AuthenticationBusiness.Models.Auths;
using Co_OwnerManagementSystem.AuthenticationBusiness.Utils;
using Co_OwnerManagementSystem.AuthenticationData.Entities;
using Co_OwnerManagementSystem.AuthenticationData.Repositories.AuthTokens;
using Co_OwnerManagementSystem.SharedLibrary.Errors;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Co_OwnerManagementSystem.AuthenticationBusiness.Services.Auth.Impl;

public class TokenService : ITokenService
{
    private readonly JwtAuthenticationOptions _authOptions;
    private readonly IAuthTokenRepository _tokenRepository;

    public TokenService(IOptions<JwtAuthenticationOptions> authOptions, IAuthTokenRepository tokenRepository)
    {
        _authOptions = authOptions.Value;
        _tokenRepository = tokenRepository;
    }
    public string GenerateAccessToken(List<Claim> clams, DateTime tokenExpireAt)
    {
        string secretKey = _authOptions.SecretKey;
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _authOptions.ValidAudience,
            Issuer = _authOptions.ValidIssuer,
            Subject = new ClaimsIdentity(clams),
            Expires = tokenExpireAt,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[64];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public DateTime GenerateAccessTokenExpiration()
    {
        return DateTime.UtcNow.AddMinutes(_authOptions.ExpiryInMinutes);
    }

    public DateTime GenerateRefreshTokenExpiration()
    {
        return DateTime.UtcNow.AddDays(_authOptions.RefreshTokenExpiryInDay);
    }

    public string ComputeSha256(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }

    public async Task InvalidateToken(string refreshToken)
    {
        var existedToken = await _tokenRepository.DbSet().FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
        if (existedToken == null)
        {
            throw new ApiException(StatusCodes.Status404NotFound, ErrorCodes.NotFound, "Token not found");
        }
        existedToken.Valid = false;
        var result = await _tokenRepository.Update(existedToken);
        if (result == null)
        {
            throw new ApiException(StatusCodes.Status500InternalServerError, ErrorCodes.Internal, "Failed when saving token");
        }
    }

    public async Task<AuthTokenResult> CreateAuthToken(User user)
    {
        {
            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, $"{user.UserId}"),
                new(ClaimTypes.Role, TypeConverter.GetRoleStringFromInt(user.RoleId))
            ];
            DateTime accessTokenExpiresAt = GenerateAccessTokenExpiration();
            DateTime refreshTokenExpiresAt = GenerateRefreshTokenExpiration();

            string refreshToken = GenerateRefreshToken();
            string accessToken = GenerateAccessToken(claims, accessTokenExpiresAt);

            AuthToken newAuthToken = new()
            {
                UserId = user.UserId,
                RefreshToken = refreshToken,
                RefreshTokenExpiredAt = refreshTokenExpiresAt,
                AccessToken = accessToken,
                ExpiredAt = accessTokenExpiresAt,
                User = user,
                Valid = true,
            };

            var listOldToken = await _tokenRepository.DbSet()
                .Where(t => t.UserId == user.UserId && t.Valid == true)
                .ToListAsync();

            foreach (var token in listOldToken)
                token.Valid = false;

            AuthToken? result = await _tokenRepository.Add(newAuthToken);
            if (result == null)
            {
                throw new ApiException(StatusCodes.Status500InternalServerError, ErrorCodes.Internal, "Failed when saving token");
            }

            return new AuthTokenResult()
            {
                AccessToken = accessToken,
                AccessTokenExpiresAt = accessTokenExpiresAt,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshTokenExpiresAt
            };
        }
    }
       
}