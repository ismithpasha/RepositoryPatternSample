
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using RepositoryPatternSample.Services.IServices.Auth;
using RepositoryPatternSample.ClientModels.Base.Helpers;
using RepositoryPatternSample.ClientModels.Validators.Auth.User;
using RepositoryPatternSample.ClientModels.Models.Admin;
using RepositoryPatternSample.ClientModels.Validators;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.User;

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]

    public class UserController : ControllerBase
    {
        private ILogger<UserController> _logger;
        private IUserService _userService { get; set; } 
        public UserController(ILogger<UserController> logger, IUserService userService )
        {
            _logger = logger;
            _userService = userService; 
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? page, int? size, byte? statusId)
        {
            var res = await _userService.GetUsers(page, size, statusId);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            //var res = await _commonService.GetById<UserListVm>(id, "spAdmin_GetUserById");
            //if (res != null)
            //{
            //    return Ok(res);
            //}

            var msg = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)msg.StatusCode, msg);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserVm model)
        {
            bool hasPhone = false;
            if (model.PhoneNumber != null && model.PhoneNumber.Length > 0) hasPhone = true;

            FluentValidation.Results.ValidationResult validationResult = new UserValidator(true, hasPhone).Validate(model);
            if (validationResult.IsValid)
            {
                var loggedInUserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value);
                model.UpdatedBy = loggedInUserId;

                ResponseModel responseModel;

                responseModel = await _userService.AddUser(model);


                if (responseModel != null && responseModel.StatusCode == StatusCodes.Status200OK)
                    return Ok(responseModel);

                return StatusCode(StatusCodes.Status409Conflict, responseModel);

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


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserVm model)
        {
            bool hasPhone = false;
            if (model.PhoneNumber != null && model.PhoneNumber.Length > 0) hasPhone = true;

            FluentValidation.Results.ValidationResult validationResult = new UserValidator(false, hasPhone).Validate(model);
            if (validationResult.IsValid)
            {
                var loggedInUserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value);
                model.UpdatedBy = loggedInUserId;

                ResponseModel responseModel;

                responseModel = await _userService.UpdateUser(model);

                if (responseModel != null && responseModel.StatusCode == StatusCodes.Status200OK)
                    return Ok(responseModel);

                return StatusCode(StatusCodes.Status409Conflict, responseModel);

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

        [HttpPut]
        [Route("Active/{id}")]
        public async Task<IActionResult> Active(int id)
        {
            var loggedInUserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value);

            var res = await _userService.ActiveUser(id, loggedInUserId, true);
            if (res != null && res.StatusCode == StatusCodes.Status200OK)
                return Ok(res);

            return StatusCode(StatusCodes.Status409Conflict, res);

        }

        [HttpPut]
        [Route("InActive/{id}")]
        public async Task<IActionResult> InActive(int id)
        {
            var loggedInUserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value);

            var res = await _userService.ActiveUser(id, loggedInUserId, false);
            if (res != null && res.StatusCode == StatusCodes.Status200OK)
                return Ok(res);

            return StatusCode(StatusCodes.Status409Conflict, res);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var loggedInUserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value);

            var res = await _userService.DeleteUser(id, loggedInUserId);
            if (res != null && res.StatusCode == StatusCodes.Status200OK)
                return Ok(res);

            return StatusCode(StatusCodes.Status409Conflict, res);

        }

        [HttpPost]
        [Route("ChangeUserPassword")]
        public async Task<IActionResult> ChangeUserPassword(ChangeUserPasswordVm changePassword)
        {
            FluentValidation.Results.ValidationResult validationResult = new ChangeUserPasswordValidator().Validate(changePassword);
            if (validationResult.IsValid)
            {
                var res = await _userService.ChangeUserPassword(changePassword);
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

    }
}
