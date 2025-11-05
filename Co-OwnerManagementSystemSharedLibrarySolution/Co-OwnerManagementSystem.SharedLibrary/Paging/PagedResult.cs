namespace Co_OwnerManagementSystem.SharedLibrary.Paging;

public record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount, int TotalPages);