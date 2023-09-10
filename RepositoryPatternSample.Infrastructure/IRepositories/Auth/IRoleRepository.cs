using RepositoryPatternSample.Entities.Domain;
using RepositoryPatternSample.Infrastructure.Core;

namespace RepositoryPatternSample.Infrastructure.IRepositories.Auth
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
    }
}
