using Co_OwnerManagementSystem.AuthenticationData.Entities;
using Co_OwnerManagementSystem.SharedLibrary.Base;

namespace Co_OwnerManagementSystem.AuthenticationData.Repositories.Users;

public interface IUserRepository : IBaseRepository<AppUser, int>
{
}