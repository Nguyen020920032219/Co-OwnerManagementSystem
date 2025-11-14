using Co_OwnerManagementSystem.AuthenticationApi.Configurations;
using Co_OwnerManagementSystem.AuthenticationBusiness.Configurations;
using Co_OwnerManagementSystem.AuthenticationData.Entities;
using Co_OwnerManagementSystem.SharedLibrary.Errors;
using Co_OwnerManagementSystem.SharedLibrary.Http;
using Co_OwnerManagementSystem.SharedLibrary.Observability;
using Microsoft.EntityFrameworkCore;

var b = WebApplication.CreateBuilder(args);
b.Services.AddEndpointsApiExplorer();
b.Services.AddSwaggerGen();
b.Services.AddControllers();

b.Services.AddStandardProblemDetails();
b.Services.AddSharedHttpInfra();

b.Services.ConfigureServices();
b.Services.ConfigureRepositories();
b.Services.ConfigureSwagger();

b.Services.AddDbContext<AuthenticationDbContext>(options =>
    options.UseSqlServer(b.Configuration.GetConnectionString("DefaultConnection")));

// Configure JwtAuthenticationOptions

b.Services.AddAuthenticationOptions(b.Configuration);
b.Services.AddAuthorization();

var app = b.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCorrelationId();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization(); // UseAuthorization should come after UseAuthentication

app.MapControllers();
app.Run();
