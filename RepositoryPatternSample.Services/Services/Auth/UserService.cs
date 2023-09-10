
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Dapper;
using System.Data;
using RepositoryPatternSample.Services.IServices.Auth;
using RepositoryPatternSample.Entities.Domain;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.ClientModels.Models.Utilities;
using static RepositoryPatternSample.ClientModels.Base.AppConstants;
using RepositoryPatternSample.Services.Base.Helpers;
using RepositoryPatternSample.ClientModels.Base.Resources;
using RepositoryPatternSample.ClientModels.Base.Helpers;
using RepositoryPatternSample.ClientModels.Models.Admin;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.User;

namespace RepositoryPatternSample.Services.Services.Auth
{
    public class UserService : IUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        public IUnitOfWork _unitOfWork;
        private readonly IDapper _dapper;
        public UserService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IDapper dapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _dapper = dapper;
        }



        public async Task<object> GetUsers(int? page, int? size, byte? statusId)
        {
            try
            {
                string queryParam = "";
                if (statusId == null) statusId = (byte)StatusId.InActive;
                if (queryParam == null || queryParam.ToString().Trim().Length == 0) queryParam = "";

                var data = new PaginatedData<UserListVm>();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("page", page);
                dynamicParameters.Add("size", size);
                dynamicParameters.Add("queryParam", queryParam);
                dynamicParameters.Add("statusId", statusId);

                data.Data = (await _dapper.GetAllAsync<UserListVm>("spAdmin_GetUsers", dynamicParameters)).ToList();
                if (data != null && data.Data.Count > 0) data.TotalRows = data.Data[0].TotalElements;
                return SPManager.FinalPasignatedResult(data, (int)page, (int)size);

            }
            catch (Exception ex)
            {
                throw;
            }
        }



        #region ------------ Command ------------ 

        public async Task<ResponseModel> AddUser(UserVm model)
        {
            try
            {
                int? typeId = null;

                if (model.TypeId != null && model.TypeId > 0)
                {
                    typeId = model.TypeId;

                    var typeValid = await _unitOfWork.UserTypeRepository.AnyAsync(x => x.Id > 0 && x.Id == typeId && x.StatusId != (byte)StatusId.Delete) != null;

                    if (typeValid == false)
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status400BadRequest,
                            Status = "Invalid",
                            Message = "User Type is not valid!"
                        };
                }
                var userExists = await _userManager.FindByNameAsync(model.UserName);
                if (userExists != null)
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Status = "Invalid",
                        Message = "User already exists!"
                    };

                var emailExists = await _userManager.FindByEmailAsync(model.Email);
                if (emailExists != null)
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Status = "Invalid",
                        Message = "Email already exists!"
                    };

                if (model.PhoneNumber != null && model.PhoneNumber.ToString().Trim().Length > 3)
                {
                    var phoneExists = await _unitOfWork.UserRepository.AnyAsync(x => x.PhoneNumber == model.PhoneNumber.Trim());
                    if (phoneExists == true)
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status409Conflict,
                            Status = "Invalid",
                            Message = "Phone already exists!"
                        };
                }

                ApplicationUser user = new()
                {
                    Name = model.Name,
                    TypeId = typeId,
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.UserName.ToLower().Trim(),
                    Address = model.Address,
                    StatusId = (byte)StatusId.Active,
                    PhoneNumber = model.PhoneNumber,
                    TempPass = true,
                    CreatedAt = DateTime.Now,
                    CreatedBy = model.UpdatedBy
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Status = "Error",
                        Message = "User creation failed! " +
                        "Please check user details and try again."
                    };

                //_cache.Remove(UserMenuCache);
                return new ResponseModel
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Status = "Success",
                    Message = "User created successfully!"
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResponseModel> UpdateUser(UserVm model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString());
                if (user == null || user.StatusId == (byte)StatusId.Delete)
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Status = "Invalid",
                        Message = "User does not exists!"
                    };

                int? typeId = null;

                if (model.TypeId != null && model.TypeId > 0)
                {
                    typeId = model.TypeId;

                    var typeValid = _unitOfWork.UserTypeRepository.AnyAsync(x => x.Id > 0 && x.Id == typeId && x.StatusId != (byte)StatusId.Delete) != null;

                    if (typeValid == false)
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status409Conflict,
                            Status = "Invalid",
                            Message = "User Type is not valid!"
                        };
                }

                var emailExists = await _userManager.FindByEmailAsync(model.Email);
                if (emailExists != null && user.Email != emailExists.Email)
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Status = "Invalid",
                        Message = "Email used by another user!"
                    };

                if (model.PhoneNumber != null && model.PhoneNumber.ToString().Trim().Length > 3)
                {
                    var phoneExists = await _unitOfWork.UserRepository.AnyAsync(
                    x => x.Id != model.Id && x.PhoneNumber != null && x.PhoneNumber == model.PhoneNumber.Trim() && x.StatusId != (byte)StatusId.Delete);
                    if (phoneExists != null && phoneExists == true)
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status409Conflict,
                            Status = "Invalid",
                            Message = "Phone already registered with another user!"
                        };
                }

                user.Name = model.Name;
                user.Email = model.Email;
                user.UpdatedBy = model.UpdatedBy;
                user.UpdatedAt = DateTime.Now;

                if (model.Address != null && model.Address.Length > 0) user.Address = model.Address;
                if (model.PhoneNumber != null && model.PhoneNumber.Length > 0) user.PhoneNumber = model.PhoneNumber;
                if (model.TypeId != null && model.TypeId > 0) user.TypeId = model.TypeId;
                user.StatusId = (byte)StatusId.Active;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Status = "Error",
                        Message = "User update failed! "
                    };


                //_cache.Remove(UserMenuCache);
                return new ResponseModel
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Status = "Success",
                    Message = "User updated successfully!",
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResponseModel> ActiveUser(int id, int userId, bool isActive)
        {
            try
            {
                string msg = "Successfully Activated!";
                byte statusId = (byte)StatusId.Active;
                if (!isActive)
                {
                    statusId = (byte)StatusId.InActive;
                    msg = "Successfully In-Activated!";
                }

                var data = await _unitOfWork.UserRepository.GetAsync(x => x.Id == id && id > 0 && x.StatusId != (byte)StatusId.Delete);



                if (data == null)
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Status = "Invalid",
                        Message = "Data not found!"
                    };


                data.StatusId = statusId;
                data.UpdatedBy = userId;
                data.UpdatedAt = DateTime.Now;
                await _unitOfWork.UserRepository.UpdateAsync(data);
                this.SaveRecord();

                return Utilities.GetSuccessMsg(CommonMessages.UpdatedSuccessfully);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ResponseModel> DeleteUser(int id, int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null || user.StatusId == (byte)StatusId.Delete)
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Status = "Invalid",
                        Message = "User does not exists!"
                    };

                user.StatusId = (byte)StatusId.Delete;
                user.UpdatedBy = userId;
                user.UpdatedAt = DateTime.Now;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Status = "Error",
                        Message = "User delete failed! "
                    };

                // _cache.Remove(UserMenuCache);
                return new ResponseModel
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Status = "Success",
                    Message = "User deleted successfully!",
                    // Data = user
                };
            }
            catch
            {
                throw;
            }
        }


        public async Task<ResponseModel> ChangeUserPassword(ChangeUserPasswordVm changePassword)
        {
            try
            {
                // Need to validate ChangeBy user 
                var isValidRequest = IsAuthorizedUser(changePassword.ChangeById);

                if (!isValidRequest)
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Status = "Failed",
                        Message = "User has no permission!"
                    };

                var user = await _userManager.FindByIdAsync(changePassword.UserId.ToString());
                if (user != null && user.StatusId != (byte)StatusId.Delete)
                {
                    var _token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, _token, changePassword.NewPassword);
                    user.SecurityStamp = Guid.NewGuid().ToString();
                    var result = await _userManager.UpdateAsync(user);
                    return new ResponseModel
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Password changed successfully!"
                    };
                }
                else
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Status = "Failed",
                        Message = "User does not exists!"
                    };
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion ------------ End User ------------ 

        private bool IsAuthorizedUser(int userId)
        {
            return true;
        }


        public void SaveRecord()
        {
            _unitOfWork.Complete();
        }


    }
}
