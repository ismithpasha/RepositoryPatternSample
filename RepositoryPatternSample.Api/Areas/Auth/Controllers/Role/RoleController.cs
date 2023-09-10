
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol.Core.Types;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using RepositoryPatternSample.Services.IServices.Auth;
using RepositoryPatternSample.ClientModels.Base.Helpers;
using static RepositoryPatternSample.ClientModels.Base.AppConstants;
using RepositoryPatternSample.ClientModels.Validators.Auth.Role;
using RepositoryPatternSample.ClientModels.Base.Resources;
using RepositoryPatternSample.ClientModels.Models.Auth.Role;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]

    public class RoleController : ControllerBase
    {

        private ILogger<RoleController> _logger;
        private IRoleService _roleService { get; set; } 
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _loggedInUserId;
        ResponseModel _response = new ResponseModel();
        public RoleController(ILogger<RoleController> logger, IRoleService roleService
                                ,  IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger; 
            _roleService = roleService;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value);

        }

        // GET: api/Role
        [HttpGet] 
        public async Task<IActionResult> GetAll(int page, int size, byte? statusId)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var res = await _roleService.GetAllRoles(page, size, statusId, null, request);

            if (res != null)
            {
                return Ok(res);
            }

            _response = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)_response.StatusCode, _response);

        }
        [HttpGet]
        [Route("GetMappedRoles")]
        public async Task<IActionResult> GetMappedRoles()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var res = await _roleService.GetMappedUnmappedRoles(true);
            if (res != null)
            {
                return Ok(res);
            }

            _response = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)_response.StatusCode, _response);


        }
        [HttpGet]
        [Route("GetUnmappedRoles")]
        public async Task<IActionResult> GetUnmappedRoles()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var res = await _roleService.GetMappedUnmappedRoles(false);
            if (res != null)
            {
                return Ok(res);
            }

            _response = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)_response.StatusCode, _response);

        }
      

        // GET: api/Role/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _roleService.GetRoleById(id);
            if (res != null)
            {
                return Ok(res);
            }

            _response = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)_response.StatusCode, _response);
        }

        // POST: api/Role
        [HttpPost]
        public async Task<IActionResult> Post(RoleCreateVm model)
        {
            var validationResult = new RoleValidator().Validate(model);

            if (validationResult.IsValid)
                _response = await _roleService.AddRole(model, _loggedInUserId);
            else
                _response = Utilities.GetValidationFailedMsg(FluentValidationHelper.GetErrorMessage(validationResult.Errors));

            return StatusCode((int)_response.StatusCode, _response);
        }

        // PUT: api/Role/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, RoleVm model)
        {
            
            var validationResult = new RoleUpdateValidator(id).Validate(model);

            if (validationResult.IsValid)
                _response = await _roleService.UpdateRole(model, _loggedInUserId);
            else
                _response = Utilities.GetValidationFailedMsg(FluentValidationHelper.GetErrorMessage(validationResult.Errors));

            return StatusCode((int)_response.StatusCode, _response);


        }

        [HttpPut, Route("Active/{id}")]
        public async Task<IActionResult> Active(int id)
        {
            if (id > 0)
                _response = await _roleService.ModifyStatus(id, (byte)StatusId.Active, _loggedInUserId);
            else
                _response = Utilities.GetValidationFailedMsg(CommonMessages.InvalidId);

            return StatusCode((int)_response.StatusCode, _response);
        }

        [HttpPut, Route("InActive/{id}")]
        public async Task<IActionResult> InActive(int id)
        {
            if (id > 0)
                _response = await _roleService.ModifyStatus(id, (byte)StatusId.InActive, _loggedInUserId);
            else
                _response = Utilities.GetValidationFailedMsg(CommonMessages.InvalidId);

            return StatusCode((int)_response.StatusCode, _response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
                _response = await _roleService.ModifyStatus(id, (byte)StatusId.Delete, _loggedInUserId);
            else
                _response = Utilities.GetValidationFailedMsg(CommonMessages.InvalidId);

            return StatusCode((int)_response.StatusCode, _response);
        }



    }
}
