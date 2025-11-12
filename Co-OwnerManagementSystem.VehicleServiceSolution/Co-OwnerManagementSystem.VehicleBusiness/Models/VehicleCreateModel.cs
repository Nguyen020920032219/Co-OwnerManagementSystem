using System.ComponentModel.DataAnnotations;
using Co_OwnerManagementSystem.VehicleInfrastructure.Enum;

namespace Co_OwnerManagementSystem.VehicleApplication.Models;

public class VehicleCreateModel
{
    [Required(ErrorMessage = "ContractId is required.")]
    public int? ContractId { get; set; }
    [Required(ErrorMessage = "LicensePlate is required.")]
    public string LicensePlate { get; set; } = null!;
    [Required(ErrorMessage = "Vin is required.")]
    public string Vin { get; set; } = null!;
    [Required(ErrorMessage = "Make is required.")]
    public string? Make { get; set; }
    [Required(ErrorMessage = "Model is required.")]
    public string? Model { get; set; }
    [Required(ErrorMessage = "Year is required.")]
    [Range(0.1, int.MaxValue, ErrorMessage = "Year must be greater than 0.")]
    public int? Year { get; set; }
    [Required(ErrorMessage = "Color is required.")]
    public string? Color { get; set; }
    [Required(ErrorMessage = "BatteryCapacity is required.")]
    [Range(0.1, double.MaxValue, ErrorMessage = "BatteryCapacity must be greater than 0.")]
    public decimal? BatteryCapacity { get; set; }
    [Required(ErrorMessage = "ChargingType is required.")]
    public string? ChargingType { get; set; }
    [Required(ErrorMessage = "PurchaseDate is required.")]
    public DateOnly? PurchaseDate { get; set; }
    [Required(ErrorMessage = "PurchasePrice is required.")]
    [Range(0.1, double.MaxValue, ErrorMessage = "PurchasePrice must be greater than 0.")]
    public decimal? PurchasePrice { get; set; }
    [Required(ErrorMessage = "CoOwnerGroupId is required.")]
    public int CoOwnerGroupId { get; set; }

    public VehicleStatus Status { get; set; } = VehicleStatus.Available;
}