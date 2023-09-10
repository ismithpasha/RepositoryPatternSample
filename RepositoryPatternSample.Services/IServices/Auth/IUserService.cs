using RepositoryPatternSample.ClientModels.Models.Admin;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.User;

namespace RepositoryPatternSample.Services.IServices.Auth
{
    public interface IUserService
    {
        Task<object> GetUsers(int? page, int? size, byte? statusId);

        Task<ResponseModel> AddUser(UserVm model);
        Task<ResponseModel> UpdateUser(UserVm model);
        Task<ResponseModel> DeleteUser(int id, int userId);
        Task<ResponseModel> ActiveUser(int id, int userId, bool isActive);
        Task<ResponseModel> ChangeUserPassword(ChangeUserPasswordVm changePassword);

    }
}
