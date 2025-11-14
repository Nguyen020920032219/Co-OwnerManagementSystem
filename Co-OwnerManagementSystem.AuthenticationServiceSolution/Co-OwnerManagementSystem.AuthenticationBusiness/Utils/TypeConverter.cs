using Co_OwnerManagementSystem.AuthenticationBusiness.Constants;

namespace Co_OwnerManagementSystem.AuthenticationBusiness.Utils;

public static class TypeConverter
{
    public static string GetRoleStringFromInt(int role)
    {
        return role switch
        {
            1 => SystemRoleConstants.Admin,
            2 => SystemRoleConstants.Staff,
            3 => SystemRoleConstants.CoOwner,
            _ => "unknown"
        };
    }
}