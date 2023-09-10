
using RepositoryPatternSample.ClientModels.Models.Common;
using RepositoryPatternSample.ClientModels.Models.Utilities;
using System.Drawing.Printing;

namespace RepositoryPatternSample.Services.Base.Helpers
{
    public static class SPManager
    { 
        public static ResponseForList FinalPasignatedResult<T>(PaginatedData<T> paginatedData, int page, int size) where T : new()
        {
            int totalElement = paginatedData.TotalRows ?? 0;
            return new ResponseForList(paginatedData.Data, totalElement, page, size);

        }

        public static ResponseForList PreparePaginatedResponse<T>(List<T> data, int totalElements, int page, int size) where T : class
        {
            return new ResponseForList(data, totalElements, page, size);
        }
    }
}
