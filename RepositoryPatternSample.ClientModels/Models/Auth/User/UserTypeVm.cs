using RepositoryPatternSample.ClientModels.Models.Common;
using RepositoryPatternSample.ClientModels.Models.Utilities;

namespace RepositoryPatternSample.ClientModels.Models.Auth.User
{
    public class UserTypeVm
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class UserTypeListVm : CommonVmProperties
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public byte StatusId { get; set; }
        public string? CreatedByUser { get; set; }
        public string? UpdatedByUser { get; set; }
        public int TotalElements { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
