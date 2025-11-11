using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Co_OwnerManagementSystem.AuthenticationData.Entities;
using Co_OwnerManagementSystem.SharedLibrary.Errors;
using Co_OwnerManagementSystem.SharedLibrary.Http;
using Co_OwnerManagementSystem.SharedLibrary.Observability;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var b = WebApplication.CreateBuilder(args);
b.Services.AddEndpointsApiExplorer();
b.Services.AddSwaggerGen();

b.Services.AddStandardProblemDetails();
b.Services.AddSharedHttpInfra();

b.Services.AddDbContext<AuthenticationDbContext>(options =>
    options.UseSqlServer(b.Configuration.GetConnectionString("DefaultConnection")));

var app = b.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCorrelationId();
app.UseExceptionHandler();

var jwt = app.Configuration.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
var issuer = jwt["Issuer"];
var audPublic = jwt["AudiencePublic"];

app.MapPost("/api/auth/login", (LoginDto dto) =>
{
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, "103"),
        new Claim("role", "CoOwner"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
    var token = new JwtSecurityToken(
        issuer,
        audPublic,
        claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
    );
    return Results.Ok(new
    {
        accessToken = new JwtSecurityTokenHandler().WriteToken(token),
        tokenType = "Bearer",
        expiresIn = 3600
    });
});

app.MapPost("/oauth/token", (ServiceTokenRequest req) =>
{
    var claims = new[]
    {
        new Claim("client_id", req.ClientId),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
    var token = new JwtSecurityToken(
        issuer,
        req.Audience,
        claims,
        expires: DateTime.UtcNow.AddMinutes(15),
        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
    );
    return Results.Ok(new
    {
        accessToken = new JwtSecurityTokenHandler().WriteToken(token),
        tokenType = "Bearer",
        expiresIn = 900
    });
});

app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();

internal record LoginDto(string Phone, string Password);

internal record ServiceTokenRequest(string ClientId, string ClientSecret, string Audience);