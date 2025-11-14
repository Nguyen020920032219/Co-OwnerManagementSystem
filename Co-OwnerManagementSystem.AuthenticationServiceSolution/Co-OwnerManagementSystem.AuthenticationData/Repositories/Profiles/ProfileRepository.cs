using Co_OwnerManagementSystem.AuthenticationData.Entities;
using Co_OwnerManagementSystem.SharedLibrary.Base;

namespace Co_OwnerManagementSystem.AuthenticationData.Repositories.Profiles;

public class ProfileRepository : BaseRepository<AuthenticationDbContext, Profile, int>, IProfileRepository
{
    public ProfileRepository(AuthenticationDbContext context) : base(context)
    {
        
    }
}