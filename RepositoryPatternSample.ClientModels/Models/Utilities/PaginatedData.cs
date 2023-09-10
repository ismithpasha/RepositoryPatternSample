namespace RepositoryPatternSample.ClientModels.Models.Utilities
{
    public class PaginatedData<T>
    {
        public List<T>? Data { get; set; }
        public int? TotalRows { get; set; }
    }
}
