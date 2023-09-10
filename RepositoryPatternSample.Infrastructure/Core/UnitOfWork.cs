
using AutoMapper;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.Entities.Data;
using RepositoryPatternSample.Infrastructure.IRepositories.Auth;
using RepositoryPatternSample.Infrastructure.Repositories.Auth;

namespace RepositoryPatternSample.Infrastructure.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDapper _dapper;
        private readonly IMapper _mapper;

        public UnitOfWork(ApplicationDbContext context, IDapper dapper, IMapper mapper) 
        {
            _dapper = dapper;
            _mapper = mapper;
            _dbContext = context;

            UserRepository = new UserRepository(_dbContext);
            UserTypeRepository = new UserTypeRepository(_dbContext); 
            RoleRepository = new RoleRepository(_dbContext);  
            MenuRepository = new MenuRepository(_dbContext); 
            RoleMenuRepository = new RoleMenuRepository(_dbContext, mapper);  

        }

        #region Area Auth
        public IUserRepository UserRepository { get; private set; }
        public IUserTypeRepository UserTypeRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }
        public IMenuRepository MenuRepository { get; private set; }
        public IRoleMenuRepository RoleMenuRepository { get; private set; }

        #endregion


        public int Complete()
        {
            return _dbContext.SaveChanges();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }

    }
}
 