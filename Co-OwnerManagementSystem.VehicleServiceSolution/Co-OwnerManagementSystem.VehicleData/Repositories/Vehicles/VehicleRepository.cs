using Co_OwnerManagementSystem.SharedLibrary.Base;
using Co_OwnerManagementSystem.VehicleInfrastructure.Entities;

namespace Co_OwnerManagementSystem.VehicleInfrastructure.Repositories.Vehicles;

public class VehicleRepository : BaseRepository<VehicleDbContext, Vehicle, int>, IVehicleRepository
{
    public VehicleRepository(VehicleDbContext dbContext) : base(dbContext)
    {
    }
}