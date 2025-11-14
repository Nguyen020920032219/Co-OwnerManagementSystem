using System.Text.Json.Serialization;
using Co_OwnerManagementSystem.SharedLibrary.Errors;
using Co_OwnerManagementSystem.SharedLibrary.Http;
using Co_OwnerManagementSystem.SharedLibrary.Observability;
using Co_OwnerManagementSystem.VehicleApplication.Mapper;
using Co_OwnerManagementSystem.VehicleApplication.Services;
using Co_OwnerManagementSystem.VehicleInfrastructure.Entities;
using Co_OwnerManagementSystem.VehicleInfrastructure.Repositories.Vehicles;
using Microsoft.EntityFrameworkCore;

var b = WebApplication.CreateBuilder(args);

b.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
b.Services.AddEndpointsApiExplorer();
b.Services.AddSwaggerGen();

b.Services.AddDbContext<VehicleDbContext>(option => option.UseSqlServer(b.Configuration.GetConnectionString("DefaultConnectionString")));

b.Services.AddScoped<IVehicleRepository, VehicleRepository> ();

b.Services.AddScoped<IVehicleService, VehicleService>();

b.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

b.Services.AddStandardProblemDetails();
b.Services.AddSharedHttpInfra();

var app = b.Build();
app.MapControllers();
app.UseCorrelationId();
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.Run();