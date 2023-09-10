using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.Role;
using RepositoryPatternSample.ClientModels.Models.Common;
using Microsoft.AspNetCore.Http;

namespace RepositoryPatternSample.Services.IServices.Auth
{
    public interface IRoleService
    {

        Task<ResponseForList> GetAllRoles(int page, int size, byte? statusId, bool? isMappedRole, HttpRequest? request);
        Task<List<DropDownVm>> GetMappedUnmappedRoles(bool isMappedRole);
        Task<RoleVm> GetRoleById(int roleId);
        Task<ResponseForList> GetAllUserRoles(int page, int size);

        Task<ResponseModel> AddRole(RoleCreateVm model, int UserId);
        Task<ResponseModel> UpdateRole(RoleVm model, int UserId);
        Task<ResponseModel> ModifyStatus(int id, byte statusId, int UserId);

        Task<ResponseModel> SaveUserRole(UserRoleVm model, int UserId);
        Task<ResponseModel> DeleteUserRole(int UserId, int RoleId);
    }
}
