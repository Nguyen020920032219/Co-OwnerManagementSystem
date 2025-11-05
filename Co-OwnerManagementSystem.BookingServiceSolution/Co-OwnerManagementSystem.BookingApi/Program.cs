using System.Text;
using Co_OwnerManagementSystem.BookingApi.Http;
using Co_OwnerManagementSystem.SharedLibrary.Abstractions;
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

b.Services.AddMemoryCache();
b.Services.AddSharedHttpInfra();
b.Services.AddSingleton<IServiceTokenProvider, ServiceTokenProvider>();

b.Services.AddHttpClient("auth-token", c =>
        c.BaseAddress = new Uri(b.Configuration["Auth:TokenEndpoint"]!))
    .WithStandardResilience(TimeSpan.FromSeconds(10));

b.Services.AddHttpClient<IVehicleClient, VehicleClient>(c =>
        c.BaseAddress = new Uri(b.Configuration["Clients:Vehicle"]!))
    .AddServiceAuth(JwtAudiences.Vehicle)
    .WithStandardResilience();

b.Services.AddHttpClient<IContractGroupClient, ContractGroupClient>(c =>
        c.BaseAddress = new Uri(b.Configuration["Clients:ContractGroup"]!))
    .AddServiceAuth(JwtAudiences.ContractGroup)
    .WithStandardResilience();

var app = b.Build();

app.UseCorrelationId();
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/ping", () => Results.Ok(new { ok = true, svc = "booking" }))
    .RequireAuthorization("public");

app.MapGet("/internal/v1/bookings/ping", () => Results.Ok(new { ok = true, svc = "booking" }))
    .RequireAuthorization("internal");

app.MapControllers().RequireAuthorization("public");
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();