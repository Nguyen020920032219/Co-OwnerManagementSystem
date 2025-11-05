using Microsoft.AspNetCore.Authorization;

namespace Co_OwnerManagementSystem.SharedLibrary.Abstractions;

public sealed class RequirePublicAttribute : AuthorizeAttribute
{
    public RequirePublicAttribute() : base(AuthPolicies.Public)
    {
    }
}

public sealed class RequireInternalAttribute : AuthorizeAttribute
{
    public RequireInternalAttribute() : base(AuthPolicies.Internal)
    {
    }
}