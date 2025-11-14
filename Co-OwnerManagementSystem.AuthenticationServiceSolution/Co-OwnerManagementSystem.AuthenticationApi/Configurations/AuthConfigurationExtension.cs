using System.Security;
using System.Text;
using System.Text.Json;
using Co_OwnerManagementSystem.AuthenticationBusiness.Configurations;
using Co_OwnerManagementSystem.SharedLibrary.Errors;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Co_OwnerManagementSystem.AuthenticationApi.Configurations;

public static class AuthConfigurationExtension
{
    public static void AddAuthenticationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        
        JwtAuthenticationOptions? jwtAuthOptions = configuration.GetSection("Authentication:JwtAuthentication")
            .Get<JwtAuthenticationOptions>();

        if (jwtAuthOptions == null) throw new ApplicationException("JWT authentication is missing from configuration");

        var secretKey = jwtAuthOptions.SecretKey;
        if (String.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
        {
            throw new SecurityException("The secret key must be at least 32 characters to avoid brute force");
        }

        services.Configure<JwtAuthenticationOptions>(configuration.GetSection("Authentication:JwtAuthentication"));
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = jwtAuthOptions.ValidateAudience,
                    ValidateLifetime = jwtAuthOptions.ValidateLifetime,
                    ValidateIssuer = jwtAuthOptions.ValidateIssuer,
                    ValidateIssuerSigningKey = jwtAuthOptions.ValidateIssuerSigningKey,
                    ValidAudience = jwtAuthOptions.ValidAudience,
                    ValidIssuer = jwtAuthOptions.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthOptions.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var path = context.HttpContext.Request.Path;
                        if (path.StartsWithSegments("/hubs"))
                        {
                            var tokenFromQuery = context.Request.Query["at"];
                            if (!string.IsNullOrEmpty(tokenFromQuery))
                            {
                                context.Token = tokenFromQuery;
                                return Task.CompletedTask;
                            }
                        }

                        string? tokenHeader = context.Request.Headers["at"];
                        if (!string.IsNullOrEmpty(tokenHeader))
                        {
                            context.Token = tokenHeader;
                            return Task.CompletedTask;
                        }

                        string? tokenCookie = context.Request.Cookies["at"];
                        if (!string.IsNullOrEmpty(tokenCookie))
                        {
                            context.Token = tokenCookie;
                        }

                        if (string.IsNullOrEmpty(context.Token))
                        {
                            var authHeader = context.Request.Headers["Authorization"].ToString();
                            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                            {
                                context.Token = authHeader["Bearer ".Length..];
                            }
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var errorResponse = new ApiException(StatusCodes.Status401Unauthorized, ErrorCodes.UnAuthorized,
                            "Token expired or invalid");
                        
                        await context.Response.WriteAsJsonAsync(errorResponse);
                    },
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var errorResponse = new ApiException(StatusCodes.Status401Unauthorized, ErrorCodes.UnAuthorized,
                            "Token missing or invalid");
                        await context.Response.WriteAsJsonAsync(errorResponse);
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var errorResponse = new ApiException(StatusCodes.Status403Forbidden, ErrorCodes.Forbidden,
                            "Permission denied");
                        await context.Response.WriteAsJsonAsync(errorResponse);
                    }
                };
            });
    }
}