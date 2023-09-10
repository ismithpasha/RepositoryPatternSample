using Dapper;
using RepositoryPatternSample.ClientModels.Models.Auth.Menu;
using RepositoryPatternSample.Entities.Data;
using RepositoryPatternSample.Entities.Domain;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.Infrastructure.IRepositories.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static RepositoryPatternSample.ClientModels.Base.AppConstants;

namespace RepositoryPatternSample.Infrastructure.Repositories.Auth
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        protected readonly ApplicationDbContext _context;
        public MenuRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }


        public async Task<object> GetLoggedInNavMenuTree(int userId)
        {
            var menus = (from pa in _context.Menus
                         where pa.StatusId == (byte)StatusId.Active
                         orderby pa.SortId, pa.Name
                         select new NavMenuVm
                         {
                             Key = pa.Id,
                             Title = pa.Name,
                             ParentId = pa.ParentId,
                             Url = pa.Url ?? "" // if pa.Url is null, set Url to an empty string 
                         }).ToList();

            var parentIds = (from pa in _context.Menus
                             where pa.StatusId == (byte)StatusId.Active && (pa.ParentId != null || pa.ParentId > 0)
                             orderby pa.SortId, pa.Name
                             select pa.ParentId
            ).ToList();

            if (menus != null)
            {
                List<NavMenuVm> menuTree = GetMenuTree(menus, null);
                return new
                {
                    Menus = menuTree,
                    parentIds = parentIds.Distinct()
                };
            }
            return null;
        }

        public async Task<object> GetAllNavMenuTree()
        {
            try
            {
                var menus = (from pa in _context.Menus
                             where pa.StatusId == (byte)StatusId.Active
                             orderby pa.SortId, pa.Name
                             select new NavMenuVm
                             {
                                 Key = pa.Id,
                                 Title = pa.Name,
                                 ParentId = pa.ParentId
                             }).ToList();

                var parentIds = (from pa in _context.Menus
                                 where pa.StatusId == (byte)StatusId.Active && (pa.ParentId != null || pa.ParentId > 0)
                                 orderby pa.SortId, pa.Name
                                 select pa.ParentId
                             ).ToList();

                if (menus != null)
                {
                    List<NavMenuVm> menuTree = GetMenuTree(menus, null);
                    return new
                    {
                        Menus = menuTree,
                        parentIds = parentIds.Distinct()

                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Private Methods
        private List<NavMenuVm> GetMenuTree(List<NavMenuVm> menus, int? parentId)
        {
            return menus.Where(x => x.ParentId == parentId)
                .Select(x => new NavMenuVm
                {
                    Key = x.Key,
                    Title = x.Title,
                    ParentId = x.ParentId,
                    Url = x.Url ?? "",
                    Children = GetMenuTree(menus, x.Key)
                }).ToList();
        }


        #endregion
    }
}
