
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using NuGet.Protocol.Core.Types;
using RepositoryPatternSample.Services.IServices.Auth;
using RepositoryPatternSample.ClientModels.Validators; 
using RepositoryPatternSample.ClientModels.Base.Helpers;
using RepositoryPatternSample.ClientModels.Validators.Auth.User;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.User;

namespace RepositoryPatternSample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginVm model)
        {
            FluentValidation.Results.ValidationResult validationResult = new LoginValidator().Validate(model);
            if (validationResult.IsValid)
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                var authResponse = await _authService.Login(model, ipAddress);

                if (authResponse == null)
                {
                    return StatusCode(StatusCodes.Status409Conflict,
                        new AuthResponseModel
                        {
                            IsSuccess = false,
                            Message = "Login Failed! User or password is invalid!"
                        });

                }
                else if (authResponse != null && authResponse.IsSuccess == false)
                {
                    return StatusCode(StatusCodes.Status409Conflict,
                       new AuthResponseModel
                       {
                           IsSuccess = false,
                           Message = authResponse.Message
                       });
                }

                return Ok(authResponse);
            }
            else
            {
                return StatusCode(StatusCodes.Status409Conflict,
                       new AuthResponseModel
                       {
                           IsSuccess = false,
                           Message = "Login Failed! User or password is invalid!"
                       });

            }

        }

        [Authorize]
        [HttpPost]
        [Route("Logout/{id}")]
        public async Task<IActionResult> Logout(int id)
        {
            if (id == null || id == 0)
                return BadRequest(new ResponseModel
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Status = "Error",
                    Message = "Invalid UserId!",
                    Data = null
                });


            await _authService.Logout(id);

            return Ok(new ResponseModel
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
                Status = "Success",
                Message = "Logged out successfully!",
                Data = null
            });
        }


        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
                return Ok(new AuthResponseModel { IsSuccess = false, Message = "Tokens must be provided" });


            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var response = await _authService.GetRefreshTokenAsync(request, ipAddress);

            return Ok(response);


        }

        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordVm changePassword)
        {
            try
            {
                FluentValidation.Results.ValidationResult validationResult = new ChangePasswordValidator().Validate(changePassword);
                if (validationResult.IsValid)
                {
                    var token = HttpContext.GetTokenAsync("access_token").Result;
                    var res = await _authService.ChangePassword(changePassword, token);
                    if (res != null && res.StatusCode == StatusCodes.Status200OK)
                        return Ok(res);


                    return StatusCode(StatusCodes.Status400BadRequest, res);
                }
                else
                {
                    return StatusCode(StatusCodes.Status409Conflict,
                         new ResponseModel
                         {
                             IsSuccess = false,
                             StatusCode = StatusCodes.Status409Conflict,
                             Status = "Error",
                             Message = "Validation Failed!",
                             Data = FluentValidationHelper.GetErrorMessage(validationResult.Errors)
                         }
                        );
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Utilities.GetInternalServerErrorMsg(ex));

            }

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                FluentValidation.Results.ValidationResult validationResult = new EmailValidator().Validate(email);
                if (validationResult.IsValid)
                {
                    var res = await _authService.ForgetPassword(email);
                    if (res != null && res.StatusCode == StatusCodes.Status200OK)
                        return Ok(res);

                    return StatusCode(StatusCodes.Status409Conflict, res);
                }
                else
                {
                    return StatusCode(StatusCodes.Status409Conflict,
                         new ResponseModel
                         {
                             IsSuccess = false,
                             StatusCode = StatusCodes.Status409Conflict,
                             Status = "Error",
                             Message = "Validation Failed!",
                             Data = FluentValidationHelper.GetErrorMessage(validationResult.Errors)
                         }
                        );
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Utilities.GetInternalServerErrorMsg(ex));

            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(ForgetPasswordVm forgetPassword)
        {
            try
            {
                FluentValidation.Results.ValidationResult validationResult = new ForgetPasswordValidator().Validate(forgetPassword);
                if (validationResult.IsValid)
                {
                    var res = await _authService.UpdatePassword(forgetPassword);
                    if (res != null && res.StatusCode == StatusCodes.Status200OK)
                        return Ok(res);


                    return StatusCode(StatusCodes.Status409Conflict, res);
                }
                else
                {
                    return StatusCode(StatusCodes.Status409Conflict,
                         new ResponseModel
                         {
                             IsSuccess = false,
                             StatusCode = StatusCodes.Status409Conflict,
                             Status = "Error",
                             Message = "Validation Failed!",
                             Data = FluentValidationHelper.GetErrorMessage(validationResult.Errors)
                         }
                        );
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Utilities.GetInternalServerErrorMsg(ex));

            }

        }



    }
}
