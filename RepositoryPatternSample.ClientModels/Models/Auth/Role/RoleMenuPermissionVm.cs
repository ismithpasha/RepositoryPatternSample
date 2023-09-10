namespace RepositoryPatternSample.ClientModels.Models.Auth.Role
{
    public class RoleMenuPermissionVm
    {
        public int RoleId { get; set; }
        public List<int> MenuIds { get; set; }
    }
}
