namespace Co_OwnerManagementSystem.BookingApi.Http;

public interface IContractGroupClient
{
    Task<bool> IsMemberAsync(int groupId, int userId, CancellationToken ct);
}

public sealed class ContractGroupClient(HttpClient http) : IContractGroupClient
{
    public async Task<bool> IsMemberAsync(int groupId, int userId, CancellationToken ct)
    {
        var r = await http.GetAsync($"/internal/v1/groups/{groupId}/validate-member?userId={userId}", ct);
        r.EnsureSuccessStatusCode();
        var dto = await r.Content.ReadFromJsonAsync<ValidateMemberDto>(ct)
                  ?? new ValidateMemberDto(false, null);
        return dto.IsMember;
    }

    private sealed record ValidateMemberDto(bool IsMember, string? RoleInGroup);
}