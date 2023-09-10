
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Http;
using Dapper;
using System.Drawing;
using System.Xml.Linq;
using AutoMapper;
using RepositoryPatternSample.ClientModels.Models.Common;
using RepositoryPatternSample.Services.IServices.Auth;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.Entities.Domain;
using RepositoryPatternSample.ClientModels.Models.Utilities;
using RepositoryPatternSample.Services.Base.Helpers;
using static RepositoryPatternSample.ClientModels.Base.AppConstants;
using RepositoryPatternSample.ClientModels.Base.Resources;
using RepositoryPatternSample.ClientModels.Base.Helpers;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.ClientModels.Models.Auth.Role;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;

namespace RepositoryPatternSample.Services.Services.Auth
{
    public class RoleService : IRoleService
    {
        private readonly IDapper _dapper;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        public IUnitOfWork _unitOfWork;
        public RoleService(RoleManager<Role> roleManager, IUnitOfWork unitOfWork, IDapper dapper, IMapper mapper)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _dapper = dapper;
            _mapper = mapper;
        }

        public async Task<ResponseForList> GetAllRoles(int page, int size, byte? statusId, bool? isMappedRole, HttpRequest? request)
        {
            try
            {
                var name = request.Query["name"].ToString();
                var description = request.Query["description"].ToString();
                var data = new PaginatedData<RoleVm>();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("pageNumber", page);
                dynamicParameters.Add("pageSize", size);
                dynamicParameters.Add("name", name);
                dynamicParameters.Add("description", description);
                dynamicParameters.Add("statusId", statusId);
                dynamicParameters.Add("isMappedRoles", isMappedRole);
                dynamicParameters.Add("total_rows", dbType: DbType.Int32, direction: ParameterDirection.Output);
                data.Data = (await _dapper.GetAllAsync<RoleVm>("spRole_GetAllRoleList", dynamicParameters)).ToList();
                data.TotalRows = dynamicParameters.Get<int>("total_rows");
                return SPManager.FinalPasignatedResult(data, page, size);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }



        public async Task<RoleVm> GetRoleById(int roleId)
        {
            try
            {
                var data = new RoleVm();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("Id", roleId);
                data = (await _dapper.GetAllAsync<RoleVm>("spRole_GetById", dynamicParameters)).FirstOrDefault();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<List<DropDownVm>> GetMappedUnmappedRoles(bool isMappedRole)
        {
            try
            {
                var data = new List<DropDownVm>();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("isMappedRoles", isMappedRole);
                data = (await _dapper.GetAllAsync<DropDownVm>("spRole_GetAllMappedUnmappedRoleList", dynamicParameters)).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ResponseForList> GetAllUserRoles(int page, int size)
        {
            try
            {
                var data = new PaginatedData<RoleVm>();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("pageNumber", page);
                dynamicParameters.Add("pageSize", size);
                dynamicParameters.Add("total_rows", dbType: DbType.Int32, direction: ParameterDirection.Output);
                data.Data = (await _dapper.GetAllAsync<RoleVm>("spRole_GetAllRoleList", dynamicParameters)).ToList();
                data.TotalRows = dynamicParameters.Get<int>("total_rows");
                return SPManager.FinalPasignatedResult(data, page, size);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }




        public async Task<ResponseModel> AddRole(RoleCreateVm model, int UserId)
        {
            try
            {
                #region Validation
                if (await _roleManager.RoleExistsAsync(model.Name))
                {
                    return Utilities.GetValidationFailedMsg(CommonMessages.DuplicateName);
                }
                #endregion

                var data = _mapper.Map<Role>(model);
                data.CreatedBy = UserId;
                data.CreatedAt = CommonMethods.GetBDCurrentTime();

                await _roleManager.CreateAsync(data);
                return Utilities.GetSuccessMsg(CommonMessages.SavedSuccessfully);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ResponseModel> UpdateRole(RoleVm model, int UserId)
        {
            try
            {
                var existingData = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == model.Id && x.StatusId != (byte)StatusId.Delete);
                model.Name = model.Name.Trim();

                #region Validation
                if (existingData == null)
                {
                    return Utilities.GetValidationFailedMsg(CommonMessages.DataDoesnotExists);
                }

                if (_roleManager.Roles.Any(x => x.Name == model.Name && x.StatusId != (byte)StatusId.Delete && x.Id != model.Id))
                {
                    return Utilities.GetValidationFailedMsg(CommonMessages.DuplicateName);
                }
                #endregion

                _mapper.Map(model, existingData);
                existingData.UpdatedBy = UserId;
                existingData.UpdatedAt = CommonMethods.GetBDCurrentTime();

                var res = await _roleManager.UpdateAsync(existingData);

                return Utilities.GetSuccessMsg(CommonMessages.SavedSuccessfully);
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
                var existingUserType = await _unitOfWork.RoleRepository.GetAsync(x => x.Id == id && x.StatusId != (byte)StatusId.Delete);

                #region Validation
                if (existingUserType == null)
                {
                    return Utilities.GetValidationFailedMsg(CommonMessages.DataDoesnotExists);
                }

                #endregion

                existingUserType.StatusId = statusId;
                existingUserType.UpdatedBy = updatedById;
                existingUserType.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.RoleRepository.UpdateAsync(existingUserType);
                SaveRecord();

                return Utilities.GetSuccessMsg(statusId == (byte)StatusId.Delete ? CommonMessages.DeletedSuccessfully : CommonMessages.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ResponseModel> SaveUserRole(UserRoleVm model, int UserId)
        {
            #region Validation
            //if (_unitOfWork.UserRoles.AnyAsync(x => x.RoleId == model.RoleId && x.UserId == model.UserId))
            //{
            //    return Utilities.GetValidationFailedMsg(ValidationMessages.UserRole_AlreadyExists);
            //} 

            if (_unitOfWork.RoleRepository.AnyAsync(x => x.Id == model.RoleId && x.StatusId == (byte)StatusId.Active) != null == false)
            {
                return Utilities.GetValidationFailedMsg(CommonMessages.DataDoesnotExists);
            }

            //if (_unitOfWork.User.Any(x => x.Id == model.UserId && x.StatusId == (byte)StatusId.Active) == false)
            //{
            //    return Utilities.GetValidationFailedMsg(ValidationMessages.User_DoesNotExists);
            //}
            #endregion

            var data = new IdentityUserRole<int>()
            {
                RoleId = model.RoleId,
                UserId = UserId
            };

            //await _unitOfWork.UserTypeRepository.AddAsync(type);
            //this.SaveRecord();
            //_context.UserRoles.Add(data);
            //_context.SaveChanges();

            return Utilities.GetSuccessMsg(CommonMessages.SavedSuccessfully);
        }

        public async Task<ResponseModel> DeleteUserRole(int UserId, int RoleId)
        {
            //var userRole = _context.UserRoles.FirstOrDefault(x => x.UserId == UserId && x.RoleId == RoleId);

            //#region Validation
            //if (userRole == null)
            //{
            //    return Utilities.GetValidationFailedMsg(CommonMessages.DataDoesnotExists);
            //}
            //#endregion

            //_context.UserRoles.Remove(userRole);
            //_context.SaveChanges();

            return Utilities.GetSuccessMsg(CommonMessages.DeletedSuccessfully);
        }



        public void SaveRecord()
        {
            _unitOfWork.Complete();
        }


    }
}
