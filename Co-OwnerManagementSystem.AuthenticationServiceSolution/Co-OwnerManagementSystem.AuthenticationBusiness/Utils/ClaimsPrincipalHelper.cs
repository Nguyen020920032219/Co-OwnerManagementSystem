using System.Net;
using System.Security.Claims;
using Co_OwnerManagementSystem.SharedLibrary.Errors;
using Microsoft.AspNetCore.Http;

namespace Co_OwnerManagementSystem.AuthenticationBusiness.Utils;

public static class ClaimsPrincipalHelper
{
    public static int GetUserId(ClaimsPrincipal? principal)
    {
        int userId;
        try
        {
            string? identifier = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            userId = int.Parse(identifier!);
        }
        catch (Exception)
        {
            throw new ApiException(StatusCodes.Status400BadRequest, ErrorCodes.InvalidCredentials, "Invalid credentials");
        }
        return userId;
    }
}