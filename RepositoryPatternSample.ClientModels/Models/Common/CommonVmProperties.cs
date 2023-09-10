

using RepositoryPatternSample.ClientModels.Base;

namespace RepositoryPatternSample.ClientModels.Models.Common
{
    public class CommonVmProperties 
    {
        public DateTime? CreatedAt { get; set; }
        public string? CreatedByName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public string? UpdatedByName { get; set; }
        public byte? StatusId { get; set; }
        public string? Status
        {
            get => _status ?? (StatusId.HasValue && Enum.IsDefined(typeof(AppConstants.StatusId), StatusId.Value) ? ((AppConstants.StatusId)StatusId.Value).ToString() : null);
            set => _status = value;
        }
        private string? _status;
    }
}
