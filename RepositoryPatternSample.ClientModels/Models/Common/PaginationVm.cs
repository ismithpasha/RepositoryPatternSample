namespace RepositoryPatternSample.ClientModels.Models.Common
{
    public abstract class PaginationVm
    {

        public int page { get; set; } = 1;
        public int size { get; set; } = 3;
        public byte? statusId { get; set; }
        public int? userid { get; set; }
    }
}