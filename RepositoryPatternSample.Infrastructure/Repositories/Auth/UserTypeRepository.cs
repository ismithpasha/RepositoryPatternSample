 
using RepositoryPatternSample.Entities.Data;
using RepositoryPatternSample.Entities.Domain;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.Infrastructure.IRepositories.Auth;

namespace RepositoryPatternSample.Infrastructure.Repositories.Auth
{
    public class UserTypeRepository : BaseRepository<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
