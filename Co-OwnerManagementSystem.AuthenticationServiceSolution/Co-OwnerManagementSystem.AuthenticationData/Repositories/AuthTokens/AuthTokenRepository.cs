using Co_OwnerManagementSystem.AuthenticationData.Entities;
using Co_OwnerManagementSystem.SharedLibrary.Base;

namespace Co_OwnerManagementSystem.AuthenticationData.Repositories.AuthTokens;

public class AuthTokenRepository : BaseRepository<AuthenticationDbContext, AuthToken, int>, IAuthTokenRepository
{
    public AuthTokenRepository(AuthenticationDbContext context) : base(context)
    {
        
    }
}
