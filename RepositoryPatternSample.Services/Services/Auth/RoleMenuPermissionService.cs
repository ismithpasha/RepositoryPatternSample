
using System.Data;
using Dapper;
using AutoMapper;
using RepositoryPatternSample.Services.IServices.Auth;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.ClientModels.Models.Common;
using RepositoryPatternSample.ClientModels.Base.Resources;
using RepositoryPatternSample.ClientModels.Base.Helpers;
using RepositoryPatternSample.Infrastructure.Core;
using static RepositoryPatternSample.ClientModels.Base.AppConstants;
using RepositoryPatternSample.ClientModels.Models.Auth.Role;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.Menu;

namespace RepositoryPatternSample.Services.Services.Auth
{
    public class RoleMenuPermissionService : IRoleMenuPermissionService
    {
        private readonly IDapper _dapper;
        private readonly IMapper _mapper;
        public IUnitOfWork _unitOfWork;
        private readonly IMenuService _menuService;
        public RoleMenuPermissionService(IMenuService menuService, IUnitOfWork unitOfWork, IDapper dapper, IMapper mapper)
        {
            _menuService = menuService;
            _unitOfWork = unitOfWork;
            _dapper = dapper;
            _mapper = mapper;
        }


        public async Task<List<MenuListVm>> GetMenusByRoleId(int roleId)
        {
            try
            {
                var data = new List<MenuListVm>();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("RoleId", roleId);
                data = (await _dapper.GetAllAsync<MenuListVm>("spMenu_GetByRoleId", dynamicParameters)).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        public async Task<List<int>> GetMenuIdsByRoleId(int roleId)
        {
            try
            {
                var data = new List<int>();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("RoleId", roleId);
                data = (await _dapper.GetAllAsync<int>("spMenu_GetMenuIdsByRoleId", dynamicParameters)).ToList();
                return data;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ResponseForList> GetRoleWiseMenuList(int? roleId)
        {
            try
            {
                return (ResponseForList)await _unitOfWork.RoleMenuRepository.GetRoleWiseMenuList(roleId);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
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


        public async Task<ResponseModel> SaveRoleMenuPermission(RoleMenuPermissionVm model, int UserId)
        {
            try
            {
                #region Validation
                model.MenuIds = model.MenuIds.Distinct().ToList();
                var role = await _unitOfWork.RoleRepository.GetAsync(x => x.Id == model.RoleId && x.StatusId != (byte)StatusId.Delete);
                if (role == null)
                {
                    return Utilities.GetValidationFailedMsg(CommonMessages.DataDoesnotExists);
                }


                var menuList = (List<MenuListVm>)await _menuService.GetMenuByIds(model.MenuIds);
                if (menuList == null || !menuList.Any() || menuList.Count != model.MenuIds.Count)
                {
                    var InvalidMenuIds = model.MenuIds.Except(menuList.Select(x => x.Id).ToList());
                    return Utilities.GetValidationFailedMsg(string.Format(CommonMessages.InvalidId, string.Join(",", InvalidMenuIds)));
                }
                #endregion
                string storedProcedureName = "spRoleMenu_SaveRoleMenu";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("RoleId", model.RoleId);
                parameters.Add("MenuIds", string.Join(",", model.MenuIds));
                parameters.Add("UserId", UserId);

                await _dapper.Execute(storedProcedureName, parameters);


                return Utilities.GetSuccessMsg(CommonMessages.SavedSuccessfully);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ResponseModel> DeleteRoleMenuPermission(int roleId, int UserId)
        {
            try
            {
                #region Validation
                if (!await _unitOfWork.RoleMenuRepository.AnyAsync(x => x.RoleId == roleId && x.StatusId != (byte)StatusId.Delete))
                {
                    return Utilities.GetValidationFailedMsg(CommonMessages.DataDoesnotExists);
                }
                #endregion
                string storedProcedureName = "spRoleMenu_DeleteRoleMenu";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("RoleId", roleId);

                await _dapper.Execute(storedProcedureName, parameters);

                return Utilities.GetSuccessMsg(CommonMessages.DeletedSuccessfully);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }



    }
}
