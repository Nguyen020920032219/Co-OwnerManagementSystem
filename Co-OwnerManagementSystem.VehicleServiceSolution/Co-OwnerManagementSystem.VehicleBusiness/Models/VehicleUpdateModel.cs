using System.ComponentModel.DataAnnotations;

namespace Co_OwnerManagementSystem.VehicleApplication.Models;

public class VehicleUpdateModel
{
    public int? ContractId { get; set; }
    public string? LicensePlate { get; set; } = null!;
    public string? Vin { get; set; } = null!;
    public string? Make { get; set; }
    public string? Model { get; set; }
    [Range(0.1, int.MaxValue, ErrorMessage = "Year must be greater than 0.")]
    public int? Year { get; set; }
    public string? Color { get; set; }
    [Range(0.1, double.MaxValue, ErrorMessage = "BatteryCapacity must be greater than 0.")]
    public decimal? BatteryCapacity { get; set; }
    public string? ChargingType { get; set; }
    public DateOnly? PurchaseDate { get; set; }
    [Range(0.1, double.MaxValue, ErrorMessage = "PurchasePrice must be greater than 0.")]
    public decimal? PurchasePrice { get; set; }
    public int? CoOwnerGroupId { get; set; }
    public int? Status { get; set; }
}