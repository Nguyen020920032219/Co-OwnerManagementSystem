using Co_OwnerManagementSystem.VehicleInfrastructure.Enum;

namespace Co_OwnerManagementSystem.VehicleApplication.Models;

public class VehicleOverallModel
{
    public int VehicleId { get; set; }
    public int? ContractId { get; set; }
    public int CoOwnerGroupId { get; set; }
    public string? Model { get; set; }
    public VehicleStatus Status { get; set; }
}