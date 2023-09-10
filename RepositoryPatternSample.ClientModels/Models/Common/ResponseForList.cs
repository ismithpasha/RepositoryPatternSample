namespace RepositoryPatternSample.ClientModels.Models.Common
{
    public class ResponseForList
    {
        public ResponseForList(object? collection, int totalElements, int page, int size)
        {
            this.Collection = collection;
            this.TotalElements = totalElements;
            this.Page = page;
            this.Size = size;

            this.TotalPages = Convert.ToInt32(Math.Ceiling((decimal)(totalElements) / size));

            if (page < this.TotalPages) this.HasNext = true;
            else this.HasNext = false;

            if (page > 1) this.HasPrev = true;
            else this.HasPrev = false;
        }
        public object? Collection { get; set; }
        public int TotalElements { get; set; } = 0;
        public int TotalPages { get; set; } = 1;
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 1;
        public bool HasNext { get; set; }
        public bool HasPrev { get; set; }
    }
}