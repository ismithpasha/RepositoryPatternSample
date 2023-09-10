using RepositoryPatternSample.ClientModels.Models.Common;
using RepositoryPatternSample.ClientModels.Models.Utilities;

namespace RepositoryPatternSample.ClientModels.Models.Auth.Menu
{ 
    public class MenuVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Url { get; set; }
        public short? SortId { get; set; }
        public string? MenuIcon { get; set; }
        public bool IsModule { get; set; }
        public bool IsParent { get; set; }
        public int? ParentId { get; set; }
        public bool? ShowMenu { get; set; }
        public bool? IsVisible { get; set; }
        public byte? LevelAt { get; set; }
    }

    public class MenuCreateVm
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Url { get; set; }
        public short? SortId { get; set; }
        public string? MenuIcon { get; set; }
        public bool IsModule { get; set; }
        public bool IsParent { get; set; }
        public int? ParentId { get; set; }
        public bool? ShowMenu { get; set; }
        public bool? IsVisible { get; set; }
        public byte? LevelAt { get; set; }
    }

    public class MenuListVm : CommonVmProperties
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Url { get; set; }
        public short? SortId { get; set; }
        public string? MenuIcon { get; set; }
        public string? Title { get; set; }
        public bool? IsModule { get; set; }
        public bool? IsParent { get; set; }
        public int? ParentId { get; set; }
        public string? ParentName { get; set; }
        public bool? ShowMenu { get; set; }
        public bool? IsVisible { get; set; }
        public byte? LevelAt { get; set; }


        public int TotalElements { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
