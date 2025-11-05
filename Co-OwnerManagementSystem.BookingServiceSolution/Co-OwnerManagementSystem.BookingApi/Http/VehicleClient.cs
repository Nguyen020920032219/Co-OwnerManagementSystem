namespace Co_OwnerManagementSystem.BookingApi.Http;

public interface IVehicleClient
{
    Task<VehicleSummaryDto> GetAsync(int vehicleId, CancellationToken ct);
}

public sealed class VehicleClient(HttpClient http) : IVehicleClient
{
    public async Task<VehicleSummaryDto> GetAsync(int vehicleId, CancellationToken ct)
    {
        var r = await http.GetAsync($"/internal/v1/vehicles/{vehicleId}", ct);
        r.EnsureSuccessStatusCode();
        return await r.Content.ReadFromJsonAsync<VehicleSummaryDto>(ct)
               ?? throw new InvalidOperationException("Vehicle response null");
    }
}

public sealed record VehicleSummaryDto(int VehicleId, int GroupId, int Status);