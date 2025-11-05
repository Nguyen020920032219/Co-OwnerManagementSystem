using System.Text;
using Co_OwnerManagementSystem.SharedLibrary.Errors;
using Co_OwnerManagementSystem.SharedLibrary.Http;
using Co_OwnerManagementSystem.SharedLibrary.Observability;
using Microsoft.IdentityModel.Tokens;

var b = WebApplication.CreateBuilder(args);

b.Services.AddReverseProxy().LoadFromConfig(b.Configuration.GetSection("ReverseProxy"));

var jwt = b.Configuration.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

b.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, ValidIssuer = jwt["Issuer"],
        ValidateAudience = true, ValidAudience = jwt["AudiencePublic"],
        ValidateIssuerSigningKey = true, IssuerSigningKey = key,
        ValidateLifetime = true
    };
});
b.Services.AddAuthorization();

b.Services.AddStandardProblemDetails();
b.Services.AddSharedHttpInfra();

var app = b.Build();

app.UseHttpsRedirection();

app.UseCorrelationId();
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy().RequireAuthorization();

app.Run();