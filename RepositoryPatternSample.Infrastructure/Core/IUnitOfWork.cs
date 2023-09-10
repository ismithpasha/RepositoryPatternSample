

using RepositoryPatternSample.Infrastructure.IRepositories.Auth;

namespace RepositoryPatternSample.Infrastructure.Core
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IUserTypeRepository UserTypeRepository { get; }
        IMenuRepository MenuRepository { get; }
        IRoleMenuRepository RoleMenuRepository { get; } 
        IRoleRepository RoleRepository { get; }   
        int Complete();
    }
}
