
namespace RepositoryPatternSample.ClientModels.Models.Admin
{
    public class ChangeUserPasswordVm
    {
        public int ChangeById { get; set; }   
        public int UserId { get; set; }
        public string NewPassword { get; set; }

      //  [Compare(nameof(NewPassword), ErrorMessage = "'New Password' and 'Confirm Password' don't match.")]
        public string ConfirmNewPassword { get; set; } 
    }
}
