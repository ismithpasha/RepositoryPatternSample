
using RepositoryPatternSample.ClientModels.Base.Helpers;
using RepositoryPatternSample.ClientModels.Base.Resources;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.User;
using RepositoryPatternSample.ClientModels.Validators.Auth.User;
using RepositoryPatternSample.Services.IServices.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using static RepositoryPatternSample.ClientModels.Base.AppConstants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class UserTypeController : ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserTypeService _userTypeService;
        ILogger<UserTypeController> _logger;
        ResponseModel response = new ResponseModel();
        private readonly int _loggedInUserId;
        public UserTypeController(IUserTypeService userTypeService, ILogger<UserTypeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _userTypeService = userTypeService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value);

        }


        [HttpGet]
        public async Task<IActionResult> Get(int page, int size, byte? statusId)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var response = await _userTypeService.GetAll(page, size, statusId, request);
            if (response != null)
            {
                return Ok(response);
            }

            var msg = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)msg.StatusCode, msg);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _userTypeService.GetUserType(id);
            if (response != null)
            {
                return Ok(response);
            }

            var msg = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)msg.StatusCode, msg);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserTypeVm model)
        {

            var validationResult = new UserTypeValidator().Validate(model);

            if (validationResult.IsValid)
                response = await _userTypeService.CreateUserType(model, _loggedInUserId);
            else
                response = Utilities.GetValidationFailedMsg(FluentValidationHelper.GetErrorMessage(validationResult.Errors));

            return StatusCode((int)response.StatusCode, response);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserTypeVm model)
        {
           
            var validationResult = new UserTypeValidator(id).Validate(model);

            if (validationResult.IsValid)
                response = await _userTypeService.UpdateUserType(model, _loggedInUserId);
            else
                response = Utilities.GetValidationFailedMsg(FluentValidationHelper.GetErrorMessage(validationResult.Errors));

            return StatusCode((int)response.StatusCode, response);

        }

        [HttpPut, Route("Active/{id}")]
        public async Task<IActionResult> Active(int id)
        {
            if (id > 0)
                response = await _userTypeService.ModifyStatus(id, (byte)StatusId.Active, _loggedInUserId);
            else
                response = Utilities.GetValidationFailedMsg(CommonMessages.InvalidId);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut, Route("InActive/{id}")]
        public async Task<IActionResult> InActive(int id)
        {
            if (id > 0)
                response = await _userTypeService.ModifyStatus(id, (byte)StatusId.InActive, _loggedInUserId);
            else
                response = Utilities.GetValidationFailedMsg(CommonMessages.InvalidId);

            return StatusCode((int)response.StatusCode, response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
                response = await _userTypeService.ModifyStatus(id, (byte)StatusId.Delete, _loggedInUserId);
            else
                response = Utilities.GetValidationFailedMsg(CommonMessages.InvalidId);

            return StatusCode((int)response.StatusCode, response);
        }


    }
}
