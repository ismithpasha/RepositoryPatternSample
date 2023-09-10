using FluentValidation;
using RepositoryPatternSample.ClientModels.Base.Helpers;
using RepositoryPatternSample.ClientModels.Base.Resources;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.Menu;
using RepositoryPatternSample.ClientModels.Validators.Auth.Menu;
using RepositoryPatternSample.Services.IServices.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static RepositoryPatternSample.ClientModels.Base.AppConstants;

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMenuService _menuService;
        ILogger<MenuController> _logger;
        ResponseModel response = new ResponseModel();
        private readonly int _loggedInUserId;
     //   private readonly ICommonService _commonService;
        public MenuController(IMenuService menuService, ILogger<MenuController> logger,
            IHttpContextAccessor httpContextAccessor )
        {
            _menuService = menuService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value);
     
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            //var res = await _commonService.GetById<MenuListVm>(id, "spMenu_GetById");
            //if (res != null)
            //{
            //    return Ok(res);
            //}

            var msg = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)msg.StatusCode, msg);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page, int size, byte? statusId)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var res = await _menuService.GetAllMenus(page, size, statusId, request);
            if (res != null)
            {
                return Ok(res);
            }

            response = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet, Route("GetLoggedInNavMenuTree")]
        public async Task<IActionResult> GetLoggedInNavMenuTree()
        {
            var res = await _menuService.GetLoggedInNavMenuTree(_loggedInUserId);
            if (res != null)
            {
                return Ok(res);
            }

            response = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet, Route("GetAllNavMenuTree")]
        public async Task<IActionResult> GetAllNavMenuTree()
        {
            var res = await _menuService.GetAllNavMenuTree();
            if (res != null)
            {
                return Ok(res);
            }

            response = Utilities.GetNoDataFoundMsg();
            return StatusCode((int)response.StatusCode, response);
        }
         

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MenuCreateVm model)
        {

            var validationResult = new MenuCreateValidator().Validate(model);

            if (validationResult.IsValid)
                response = await _menuService.Create(model, _loggedInUserId);
            else
                response = Utilities.GetValidationFailedMsg(FluentValidationHelper.GetErrorMessage(validationResult.Errors));

            return StatusCode((int)response.StatusCode, response);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MenuVm model)
        {

            var validationResult = new MenuUpdateValidator(id).Validate(model);

            if (validationResult.IsValid)
                response = await _menuService.Update(model, _loggedInUserId);
            else
                response = Utilities.GetValidationFailedMsg(FluentValidationHelper.GetErrorMessage(validationResult.Errors));

            return StatusCode((int)response.StatusCode, response);

        }

        [HttpPut, Route("Active/{id}")]
        public async Task<IActionResult> Active(int id)
        {
            if (id > 0)
                response = await _menuService.ModifyStatus(id, (byte)StatusId.Active, _loggedInUserId);
            else
                response = Utilities.GetValidationFailedMsg(CommonMessages.InvalidId);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut, Route("InActive/{id}")]
        public async Task<IActionResult> InActive(int id)
        {
            if (id > 0)
                response = await _menuService.ModifyStatus(id, (byte)StatusId.InActive, _loggedInUserId);
            else
                response = Utilities.GetValidationFailedMsg(CommonMessages.InvalidId);

            return StatusCode((int)response.StatusCode, response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
                response = await _menuService.ModifyStatus(id, (byte)StatusId.Delete, _loggedInUserId);
            else
                response = Utilities.GetValidationFailedMsg(CommonMessages.InvalidId);

            return StatusCode((int)response.StatusCode, response);
        }


    }
}
