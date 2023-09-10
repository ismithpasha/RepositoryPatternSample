using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.User;
using RepositoryPatternSample.ClientModels.Models.Common;
using RepositoryPatternSample.Entities.Domain;
using Microsoft.AspNetCore.Http;

namespace RepositoryPatternSample.Services.IServices.Auth
{
    public interface IUserTypeService
    {

        Task<ResponseForList> GetAll(int page, int size, byte? statusId, HttpRequest? request);
        Task<UserType> GetUserType(int id);
        Task<ResponseModel> CreateUserType(UserTypeVm userType, int loggedInUserId);
        Task<ResponseModel> UpdateUserType(UserTypeVm userType, int loggedInUserId);
        Task<ResponseModel> ModifyStatus(int id, byte statusId, int loggedInUserId);
        Task<bool> CheckIsExits(UserTypeVm userType);
        void SaveRecord();
    }
}
