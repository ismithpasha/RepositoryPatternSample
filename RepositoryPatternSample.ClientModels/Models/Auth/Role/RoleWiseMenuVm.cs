using RepositoryPatternSample.ClientModels.Models.Auth.Menu;

namespace RepositoryPatternSample.ClientModels.Models.Auth.Role
{
    public class RoleWiseMenuVm
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public List<NavMenuDetailsVm> Menus { get; set; }
    }
}
