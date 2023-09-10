namespace RepositoryPatternSample.ClientModels.Models.Auth.Authenticate
{
    public class ChangePasswordVm
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }

}
