namespace RepositoryPatternSample.ClientModels.Models.Auth.Menu
{
    public class NavMenuVm
    {
        public int Key { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public string Url { get; set; }
        public List<NavMenuVm>? Children { get; set; }
    }

    public class NavMenuDetailsVm
    {
        public int Key { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public string Url { get; set; }
        public string? MenuIcon { get; set; }
        public bool ShowMenu { get; set; }
        public bool IsVisible { get; set; }
        public byte? LevelAt { get; set; }
        public short? SortId { get; set; }
        public List<NavMenuDetailsVm>? Children { get; set; }
    }
}
