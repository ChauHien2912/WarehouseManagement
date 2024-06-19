using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHouseManagement.API.Constant;
using WareHouseManagement.Repository.Dtos.Request.Role;
using WareHouseManagement.Repository.Services.IServices;

namespace WareHouseManagement.API.Controllers
{
    [Route(APIEndPointConstant.Role.RoleEndPoint + "/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetRole([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _service.GetRoles(page, size);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromQuery]CreateRoleRequest request)
        {
           return Ok(await _service.CreateRole(request));
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateRole(int id, [FromQuery]UpdateRoleRequest request)
        {
            bool result = await _service.UpdateRoleById(id, request);
            if(result == false)
            {
                return NotFound("Update Fail");
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = _service.DeleteRole(id);
            return Ok(result);
        }
    }
}
