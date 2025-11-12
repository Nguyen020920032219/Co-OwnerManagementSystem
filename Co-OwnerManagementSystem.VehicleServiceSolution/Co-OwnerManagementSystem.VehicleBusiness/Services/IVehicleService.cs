using Co_OwnerManagementSystem.SharedLibrary.Paging;
using Co_OwnerManagementSystem.VehicleApplication.Models;
using Co_OwnerManagementSystem.VehicleInfrastructure.Enum;

namespace Co_OwnerManagementSystem.VehicleApplication.Services;

public interface IVehicleService
{
    Task<PagedResult<VehicleOverallModel>> GetVehicles(int pageNumber, int pageSize, string? model, VehicleStatus? status);
    Task<VehicleModel> GetVehicle(int id);
    Task<VehicleModel> CreateVehicle(VehicleCreateModel vehicleModel);
    Task<VehicleModel> UpdateVehicle(int id, VehicleUpdateModel vehicleModel);
    Task<bool> ToggleVehicle(int id);
}