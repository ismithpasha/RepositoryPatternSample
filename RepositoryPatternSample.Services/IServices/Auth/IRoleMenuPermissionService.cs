using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.Menu;
using RepositoryPatternSample.ClientModels.Models.Auth.Role;
using RepositoryPatternSample.ClientModels.Models.Common;
using Microsoft.AspNetCore.Http;

namespace RepositoryPatternSample.Services.IServices.Auth
{
    public interface IRoleMenuPermissionService
    {
        Task<List<MenuListVm>> GetMenusByRoleId(int roleId);
        Task<List<int>> GetMenuIdsByRoleId(int roleId);
        Task<ResponseForList> GetRoleWiseMenuList(int? roleId);

        Task<ResponseModel> SaveRoleMenuPermission(RoleMenuPermissionVm model, int UserId);
        Task<ResponseModel> DeleteRoleMenuPermission(int roleId, int UserId);

    }
}
