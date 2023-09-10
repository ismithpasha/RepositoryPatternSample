using RepositoryPatternSample.ClientModels.Models.Common;
using RepositoryPatternSample.Entities.Domain;
using RepositoryPatternSample.Infrastructure.Core;

namespace RepositoryPatternSample.Infrastructure.IRepositories.Auth
{
    public interface IRoleMenuRepository : IBaseRepository<RoleMenuPermission>
    {
        Task<ResponseForList> GetRoleWiseMenuList(int? roleId);
    }
}
