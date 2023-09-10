using RepositoryPatternSample.ClientModels.Models;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.Menu;
using Microsoft.AspNetCore.Http;

namespace RepositoryPatternSample.Services.IServices.Auth
{
    public interface IMenuService
    {
        Task<object> GetAllMenus(int page, int size, byte? statusId, HttpRequest? request);
        Task<object> GetLoggedInNavMenuTree(int userId);
        Task<object> GetAllNavMenuTree();

        Task<object> GetMenuByIds(List<int> ids);

        Task<ResponseModel> Create(MenuCreateVm model, int userId);
        Task<ResponseModel> Update(MenuVm model, int userId);
        Task<ResponseModel> ModifyStatus(int id, byte statusId, int userId);
    }
}
