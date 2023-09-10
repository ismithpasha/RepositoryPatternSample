 
using RepositoryPatternSample.Entities.Data;
using RepositoryPatternSample.Entities.Domain;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.Infrastructure.IRepositories.Auth;

namespace RepositoryPatternSample.Infrastructure.Repositories.Auth
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
