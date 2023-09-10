
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using RepositoryPatternSample.ClientModels.Models.Common;
using RepositoryPatternSample.Entities.Data;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.Entities.Domain;
using static RepositoryPatternSample.ClientModels.Base.AppConstants;
using RepositoryPatternSample.Infrastructure.IRepositories.Auth;
using RepositoryPatternSample.ClientModels.Models.Auth.Role;
using RepositoryPatternSample.ClientModels.Models.Auth.Menu;

namespace RepositoryPatternSample.Infrastructure.Repositories.Auth
{
    public class RoleMenuRepository : BaseRepository<RoleMenuPermission>, IRoleMenuRepository
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;
        public RoleMenuRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _context = dbContext;
            _mapper = mapper;
        }


        public async Task<ResponseForList> GetRoleWiseMenuList(int? roleId)
        {
            var result = new List<RoleWiseMenuVm>();

            var allMenusVm = _mapper.Map<List<NavMenuDetailsVm>>(_context.Menus.ToList());
            var rolePermissionQuery = _context.RoleMenuPermissions.Include(x => x.ApplicationRole).Where(x => x.StatusId == (byte)StatusId.Active);
            var rolePermission = roleId != null ? rolePermissionQuery.Where(x => x.RoleId == roleId).ToList() : rolePermissionQuery.ToList();

            if (allMenusVm != null && allMenusVm.Count > 0 && rolePermission != null && rolePermission.Count > 0)
            {
                foreach (var item in rolePermission)
                {
                    var res = result.FirstOrDefault(x => x.RoleId == item.RoleId);
                    if (res == null)
                    {
                        result.Add(new RoleWiseMenuVm()
                        {
                            RoleId = item.RoleId,
                            RoleDescription = item.ApplicationRole.Description,
                            RoleName = item.ApplicationRole.Name,
                            Menus = GetMenuWithParents(item.MenuId, allMenusVm)
                        });
                    }
                    else
                    {
                        var menus = GetMenuWithParents(item.MenuId, allMenusVm);
                        var newItems = menus.Where(x => !res.Menus.Any(y => x.Key == y.Key));
                        res.Menus.AddRange(newItems);
                    }
                }

                foreach (var item in result)
                {
                    item.Menus = BuildTree(item.Menus);
                }
            }
            return new ResponseForList(result, result.Count, 1, result.Count);
        }

        private List<NavMenuDetailsVm> GetMenuWithParents(int menuId, List<NavMenuDetailsVm> menus)
        {
            var menu = menus.FirstOrDefault(m => m.Key == menuId);

            if (menu == null)
            {
                return new List<NavMenuDetailsVm>();
            }

            var menuList = new List<NavMenuDetailsVm> { menu };

            if (menu.ParentId.HasValue)
            {
                List<NavMenuDetailsVm> parentMenus = GetMenuWithParents(menu.ParentId.Value, menus);
                menuList.InsertRange(0, parentMenus);
            }

            return menuList;
        }

        public List<NavMenuDetailsVm> BuildTree(List<NavMenuDetailsVm> items)
        {
            var tree = new List<NavMenuDetailsVm>();
            var mappedArr = new Dictionary<int, NavMenuDetailsVm>();

            // Build a hash table and map items to objects
            foreach (var item in items)
            {
                var id = item.Key;
                if (!mappedArr.ContainsKey(id)) // in case of duplicates
                {
                    mappedArr[id] = item; // the extracted id as key, and the item as value
                    mappedArr[id].Children = new List<NavMenuDetailsVm>(); // under each item, add a key "Children" with an empty list as value
                }
            }

            // Loop over hash table
            foreach (var id in mappedArr.Keys)
            {
                var mappedElem = mappedArr[id];

                // If the element is not at the root level, add it to its parent list of children. Note this will continue till we have only root level elements left
                if (mappedElem.ParentId != null)
                {
                    var parentId = mappedElem.ParentId;
                    mappedArr[(int)parentId].Children.Add(mappedElem);
                }

                // If the element is at the root level, directly add it to the tree
                else
                {
                    tree.Add(mappedElem);
                }
            }

            return tree;
        }

    }
}
