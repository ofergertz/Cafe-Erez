using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Model.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CafeErez.Server.Controllers
{
    [ApiController]
    [Route("api/identity/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get All registered users
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }
    }
}
