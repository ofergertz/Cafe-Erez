using BusinessService.Roles;
using CafeErez.Shared.BusinessService.Roles;
using Microsoft.AspNetCore.Mvc;

namespace CafeErez.Server.Controllers
{
    [ApiController]
    [Route("api/identity/roleClaim")]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _roleService;

        public RolesController(IRolesService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<IActionResult> GetAll()
        {
            var roleClaims = await _roleService.GetAllAsync();
            return Ok(roleClaims);
        }

        [HttpPost]
        [Route("SaveRole")]
        public async Task<IActionResult> SaveRole(RoleRequest RoleRequest)
        {
            var roleClaims = await _roleService.SaveAsync(RoleRequest);
            return Ok(roleClaims);

        }

        [HttpDelete("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var response = await _roleService.DeleteAsync(id);
            return Ok(response);
        }
    }
}
