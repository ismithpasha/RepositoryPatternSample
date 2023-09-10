 
using RepositoryPatternSample.Entities.Data;
using RepositoryPatternSample.Entities.Domain;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.Infrastructure.IRepositories.Auth;

namespace RepositoryPatternSample.Infrastructure.Repositories.Auth
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
