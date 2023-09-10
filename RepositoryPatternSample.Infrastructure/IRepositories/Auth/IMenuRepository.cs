using RepositoryPatternSample.Entities.Domain;
using RepositoryPatternSample.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.Infrastructure.IRepositories.Auth
{
    public interface IMenuRepository : IBaseRepository<Menu>
    {
        Task<object> GetLoggedInNavMenuTree(int userId);
        Task<object> GetAllNavMenuTree();
    }
}