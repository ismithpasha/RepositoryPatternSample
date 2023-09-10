
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Dapper;
using RepositoryPatternSample.Services.IServices.Auth;
using RepositoryPatternSample.Entities.Domain;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.ClientModels.Models.Common;
using RepositoryPatternSample.ClientModels.Models.Admin;
using static RepositoryPatternSample.ClientModels.Base.AppConstants;
using RepositoryPatternSample.ClientModels.Base.Helpers;
using RepositoryPatternSample.ClientModels.Base.Resources;
using RepositoryPatternSample.ClientModels.Models.Utilities;
using RepositoryPatternSample.Services.Base.Helpers;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.User;

namespace RepositoryPatternSample.Services.Services.Auth
{
    public class UserTypeService : IUserTypeService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDapper _dapper;

        public UserTypeService(IUnitOfWork unitOfWork, IMapper mapper, IDapper dapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dapper = dapper;
        }
        #region Query
        public async Task<UserType> GetUserType(int id)
        {
            try
            {
                return await _unitOfWork.UserTypeRepository.GetAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ResponseForList> GetAll(int page, int size, byte? statusId, HttpRequest request)
        {
            try
            {
                if (statusId == null) statusId = (byte)StatusId.Active;
                var name = request.Query["name"].ToString();
                var description = request.Query["description"].ToString();

                var data = new PaginatedData<UserTypeListVm>();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("page", page);
                dynamicParameters.Add("size", size);
                dynamicParameters.Add("statusId", statusId);
                dynamicParameters.Add("name", name);
                dynamicParameters.Add("description", description);

                data.Data = (await _dapper.GetAllAsync<UserTypeListVm>("spAdmin_GetUserTypes", dynamicParameters)).ToList();

                if (data != null && data.Data.Count > 0) data.TotalRows = data.Data[0].TotalElements;
                return SPManager.FinalPasignatedResult(data, page, size);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        public async Task<bool> CheckIsExits(UserTypeVm model)
        {
            return await _unitOfWork.UserTypeRepository.GetAsync(x => x.Name == model.Name && x.Id != model.Id) != null;
        }
        public async Task<bool> CheckIsDuplicate(UserTypeVm model)
        {
            return await _unitOfWork.UserTypeRepository.AnyAsync(x => x.Name == model.Name && x.StatusId != (byte)StatusId.Delete && x.Id != model.Id);
        }



        #endregion Query

        #region Command
        public async Task<ResponseModel> CreateUserType(UserTypeVm model, int loggedInUserId)
        {
            try
            {
                if (await CheckIsExits(model))
                {
                    return Utilities.GetAlreadyExistMsg("User Type");

                }

                var type = _mapper.Map<UserType>(model);
                type.CreatedBy = loggedInUserId;
                type.CreatedAt = DateTime.UtcNow;
                await _unitOfWork.UserTypeRepository.AddAsync(type);
                SaveRecord();
                return Utilities.GetSuccessMsg(CommonMessages.SavedSuccessfully);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        public async Task<ResponseModel> UpdateUserType(UserTypeVm model, int loggedInUserId)
        {
            try
            {

                var existingUserType = await _unitOfWork.UserTypeRepository.GetAsync(x => x.Id == model.Id && x.StatusId != (byte)StatusId.Delete);

                #region Validation
                if (existingUserType == null)
                {
                    return Utilities.GetValidationFailedMsg(CommonMessages.DataDoesnotExists);
                }

                if (await CheckIsDuplicate(model))
                {
                    return Utilities.GetAlreadyExistMsg("User Type");
                }
                #endregion

                _mapper.Map(model, existingUserType);
                existingUserType.UpdatedBy = loggedInUserId;
                existingUserType.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.UserTypeRepository.UpdateAsync(existingUserType);
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
                var existingUserType = await _unitOfWork.UserTypeRepository.GetAsync(x => x.Id == id && x.StatusId != (byte)StatusId.Delete);

                #region Validation
                if (existingUserType == null)
                {
                    return Utilities.GetValidationFailedMsg(CommonMessages.DataDoesnotExists);
                }

                #endregion

                existingUserType.StatusId = statusId;
                existingUserType.UpdatedBy = updatedById;
                existingUserType.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.UserTypeRepository.UpdateAsync(existingUserType);
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
