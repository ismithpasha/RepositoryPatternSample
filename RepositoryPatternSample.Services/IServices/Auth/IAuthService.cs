using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.User;

namespace RepositoryPatternSample.Services.IServices.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseModel> Login(LoginVm model, string ipAddress);
        Task<AuthResponseModel> GetRefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
        Task<ResponseModel> ChangePassword(ChangePasswordVm changePassword, string token);
        Task<ResponseModel> ForgetPassword(string email);
        Task<ResponseModel> UpdatePassword(ForgetPasswordVm forgetPassword);
        Task<bool> Logout(int id);
    }
}
