using Co_OwnerManagementSystem.AuthenticationBusiness.Services.Auth;
using Co_OwnerManagementSystem.AuthenticationBusiness.Services.Auth.Impl;
using Co_OwnerManagementSystem.AuthenticationData.Repositories.AuthTokens;
using Co_OwnerManagementSystem.AuthenticationData.Repositories.Profiles;
using Co_OwnerManagementSystem.AuthenticationData.Repositories.Users;
using Microsoft.AspNetCore.Identity;

namespace Co_OwnerManagementSystem.AuthenticationApi.Configurations;

public static class ServiceConfigurationExtension
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();

    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthTokenRepository, AuthTokenRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
    }
}