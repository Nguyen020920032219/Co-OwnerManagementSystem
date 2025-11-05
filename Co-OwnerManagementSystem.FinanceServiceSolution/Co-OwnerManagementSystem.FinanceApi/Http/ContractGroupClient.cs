namespace Co_OwnerManagementSystem.FinanceApi.Http;

public interface IContractGroupClient
{
    Task<GroupMembersDto> GetMembersAsync(int groupId, CancellationToken ct);
}

public sealed class ContractGroupClient(HttpClient http) : IContractGroupClient
{
    public async Task<GroupMembersDto> GetMembersAsync(int groupId, CancellationToken ct)
    {
        var r = await http.GetAsync($"/internal/v1/groups/{groupId}/members", ct);
        r.EnsureSuccessStatusCode();
        return await r.Content.ReadFromJsonAsync<GroupMembersDto>(ct)
               ?? new GroupMembersDto(groupId, new List<MemberItem>(), 1);
    }
}

public sealed record GroupMembersDto(int GroupId, List<MemberItem> Members, int Version);

public sealed record MemberItem(int UserId, double OwnershipPercent, string RoleInGroup, bool Active);