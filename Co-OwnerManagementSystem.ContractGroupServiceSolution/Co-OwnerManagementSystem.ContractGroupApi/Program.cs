using System.Text;
using Co_OwnerManagementSystem.SharedLibrary.Errors;
using Co_OwnerManagementSystem.SharedLibrary.Http;
using Co_OwnerManagementSystem.SharedLibrary.Observability;
using Microsoft.IdentityModel.Tokens;

var b = WebApplication.CreateBuilder(args);

b.Services.AddControllers();
b.Services.AddEndpointsApiExplorer();
b.Services.AddSwaggerGen();

var jwt = b.Configuration.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
var issuer = jwt["Issuer"];
var audPublic = jwt["AudiencePublic"];
var audInternal = jwt["AudienceInternal"];

b.Services.AddAuthentication()
    .AddJwtBearer("Public", o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, ValidIssuer = issuer,
            ValidateAudience = true, ValidAudience = audPublic,
            ValidateIssuerSigningKey = true, IssuerSigningKey = key,
            ValidateLifetime = true
        };
    })
    .AddJwtBearer("Internal", o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, ValidIssuer = issuer,
            ValidateAudience = true, ValidAudience = audInternal,
            ValidateIssuerSigningKey = true, IssuerSigningKey = key,
            ValidateLifetime = true
        };
    });

b.Services.AddAuthorization(o =>
{
    o.AddPolicy("public", p => p.AddAuthenticationSchemes("Public").RequireAuthenticatedUser());
    o.AddPolicy("internal",
        p => p.AddAuthenticationSchemes("Internal").RequireAuthenticatedUser().RequireClaim("client_id"));
});

b.Services.AddStandardProblemDetails();
b.Services.AddSharedHttpInfra();

var app = b.Build();

app.UseCorrelationId();
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/ping", () => Results.Ok(new { ok = true, svc = "contract-group" }))
    .RequireAuthorization("public");

app.MapGet("/internal/v1/groups/ping", () => Results.Ok(new { ok = true, svc = "contract-group" }))
    .RequireAuthorization("internal");

app.MapControllers().RequireAuthorization("public");
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();