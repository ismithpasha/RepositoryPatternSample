﻿namespace RepositoryPatternSample.ClientModels.Models.Auth.Authenticate
{
    public class ForgetPasswordVm
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
