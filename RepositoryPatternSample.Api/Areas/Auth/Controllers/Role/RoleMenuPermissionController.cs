using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RepositoryPatternSample.Services.IServices.Auth;
using RepositoryPatternSample.ClientModels.Base.Helpers;
using RepositoryPatternSample.ClientModels.Validators.Auth.Menu;
using RepositoryPatternSample.ClientModels.Base.Resources;
using RepositoryPatternSample.ClientModels.Models.Auth.Role;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]

    public class RoleMenuPermissionController : ControllerBase
    {

        private ILogger<RoleController> _logger;
        private IRoleMenuPermissionService _roleMenuService { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _loggedInUserId;
        ResponseModel _response = new ResponseModel();
        public RoleMenuPermissionController(ILogger<RoleController> logger, IRoleMenuPermissionService roleMenuService
                                , IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _roleMenuService = roleMenuService;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value);

        }


        [HttpGet, Route("GetMenusByRoleId/{roleId}")] 
        public async Task<IActionResult> GetMenusByRoleId(int roleId)
        { 
            var res = await _roleMenuService.GetMenusByRoleId(roleId);

            if (res != null)
            {
                return Ok(res);
            }

            _response = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)_response.StatusCode, _response);

        }
        [HttpGet, Route("GetMenuIdsByRoleId/{roleId}")] 
        public async Task<IActionResult> GetMenuIdsByRoleId(int roleId)
        { 
            var res = await _roleMenuService.GetMenuIdsByRoleId(roleId);

            if (res != null)
            {
                return Ok(res);
            }
            _response = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)_response.StatusCode, _response);
        }

        [HttpGet]
        [Route("GetRoleWiseMenuList")]
        public async Task<IActionResult> GetRoleWiseMenuList(int? roleId = null)
        { 
            var res = await _roleMenuService.GetRoleWiseMenuList(roleId);
            if (res != null)
            {
                return Ok(res);
            }

            _response = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)_response.StatusCode, _response);


        }


        [HttpPost]
        public async Task<IActionResult> Post(RoleMenuPermissionVm model)
        {
            var validationResult = new RoleMenuPermissionValidator().Validate(model);

            if (validationResult.IsValid)
                _response = await _roleMenuService.SaveRoleMenuPermission(model, _loggedInUserId);
            else
                _response = Utilities.GetValidationFailedMsg(FluentValidationHelper.GetErrorMessage(validationResult.Errors));

            return StatusCode((int)_response.StatusCode, _response);
        }
         

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> Delete(int roleId)
        {
            if (roleId > 0)
                _response = await _roleMenuService.DeleteRoleMenuPermission(roleId, _loggedInUserId);
            else
                _response = Utilities.GetValidationFailedMsg(CommonMessages.InvalidId);

            return StatusCode((int)_response.StatusCode, _response);
        }



    }
}
