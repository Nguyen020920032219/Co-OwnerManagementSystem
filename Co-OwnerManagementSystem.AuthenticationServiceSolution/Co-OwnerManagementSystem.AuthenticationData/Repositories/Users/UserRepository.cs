using Co_OwnerManagementSystem.AuthenticationData.Entities;
using Co_OwnerManagementSystem.SharedLibrary.Base;

namespace Co_OwnerManagementSystem.AuthenticationData.Repositories.Users;

public class UserRepository : BaseRepository<AuthenticationDbContext, User, int>, IUserRepository
{
    public UserRepository(AuthenticationDbContext context) : base(context)
    {
    }
}