
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Dapper;
using RepositoryPatternSample.Services.IServices.Auth;
using RepositoryPatternSample.Infrastructure.Core;
using static RepositoryPatternSample.ClientModels.Base.AppConstants;
using RepositoryPatternSample.ClientModels.Base.Helpers;
using RepositoryPatternSample.Entities.Domain;
using RepositoryPatternSample.ClientModels.Base.Resources;
using RepositoryPatternSample.Services.Base.Helpers;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.Menu;

namespace RepositoryPatternSample.Services.Services.Auth
{
    public class MenuService : IMenuService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDapper _dapper;

        public MenuService(IUnitOfWork unitOfWork, IMapper mapper, IDapper dapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dapper = dapper;
        }
        #region Query 

        public async Task<object> GetAllMenus(int page, int size, byte? statusId, HttpRequest? request)
        {
            var name = request.Query["name"].ToString();
            var code = request.Query["code"].ToString();
            var url = request.Query["url"].ToString();

            var data = new List<MenuListVm>();
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("page", page);
            dynamicParameters.Add("size", size);
            dynamicParameters.Add("statusId", statusId);
            dynamicParameters.Add("name", name);
            dynamicParameters.Add("code", code);
            dynamicParameters.Add("url", url);
            data = (await _dapper.GetAllAsync<MenuListVm>("sp_GetMenus", dynamicParameters)).ToList();

            if (data.Count > 0)
            {
                return SPManager.PreparePaginatedResponse(data, data[0].TotalElements, page, size);
            }
            return null;

        }


        public async Task<object> GetLoggedInNavMenuTree(int userId)
        {
            try
            {
                return _unitOfWork.MenuRepository.GetLoggedInNavMenuTree(userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }


        }

        public async Task<object> GetMenuByIds(List<int> ids)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            string menuIds = string.Join(",", ids);
            dynamicParameters.Add("Ids", menuIds);

            var res = await _dapper.GetAllAsync<MenuListVm>("spMenu_GetMenuByIdList", dynamicParameters);
            return res;
        }

        public async Task<object> GetAllNavMenuTree()
        {
            try
            {
                return _unitOfWork.MenuRepository.GetAllNavMenuTree().Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public async Task<bool> CheckIsExits(MenuCreateVm model)
        {
            return await _unitOfWork.MenuRepository.GetAsync(x => x.Name == model.Name && x.StatusId != (byte)StatusId.Delete) != null;
        }
        public async Task<bool> CheckIsDuplicate(MenuVm model)
        {
            return await _unitOfWork.MenuRepository.AnyAsync(x => x.Name == model.Name && x.StatusId != (byte)StatusId.Delete && x.Id != model.Id);
        }



        #endregion Query

        #region Command
        public async Task<ResponseModel> Create(MenuCreateVm model, int loggedInUserId)
        {
            try
            {
                if (await CheckIsExits(model))
                {
                    return Utilities.GetAlreadyExistMsg("Menu");

                }

                var type = _mapper.Map<Menu>(model);
                if (model.ParentId == 0) type.ParentId = null;
                type.CreatedBy = loggedInUserId;
                type.CreatedAt = DateTime.UtcNow;
                await _unitOfWork.MenuRepository.AddAsync(type);
                SaveRecord();
                return Utilities.GetSuccessMsg(CommonMessages.SavedSuccessfully);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        public async Task<ResponseModel> Update(MenuVm model, int loggedInUserId)
        {
            try
            {

                var existingData = await _unitOfWork.MenuRepository.GetAsync(x => x.Id == model.Id && x.StatusId != (byte)StatusId.Delete);

                #region Validation
                if (existingData == null)
                {
                    return Utilities.GetValidationFailedMsg(CommonMessages.DataDoesnotExists);
                }

                if (await CheckIsDuplicate(model))
                {
                    return Utilities.GetAlreadyExistMsg("Menu");
                }
                #endregion

                _mapper.Map(model, existingData);
                if (model.ParentId == 0) existingData.ParentId = null;
                existingData.UpdatedBy = loggedInUserId;
                existingData.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.MenuRepository.UpdateAsync(existingData);
                SaveRecord();

                return Utilities.GetSuccessMsg(CommonMessages.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public async Task<ResponseModel> ModifyStatus(int id, byte statusId, int updatedById)
        {
            try
            {
                var existingData = await _unitOfWork.MenuRepository.GetAsync(x => x.Id == id && x.StatusId != (byte)StatusId.Delete);

                #region Validation
                if (existingData == null)
                {
                    return Utilities.GetValidationFailedMsg(CommonMessages.DataDoesnotExists);
                }

                #endregion

                existingData.StatusId = statusId;
                existingData.UpdatedBy = updatedById;
                existingData.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.MenuRepository.UpdateAsync(existingData);
                SaveRecord();

                return Utilities.GetSuccessMsg(statusId == (byte)StatusId.Delete ? CommonMessages.DeletedSuccessfully : CommonMessages.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }



        public void SaveRecord()
        {
            _unitOfWork.Complete();
        }

        #endregion Command
    }
}
