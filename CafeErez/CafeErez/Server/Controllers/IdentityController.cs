using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Model.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CafeErez.Server.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Get login request data (Email, Password)
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {
            var response = await _identityService.LoginAsync(loginRequest);
            return Ok(response);
        }

        /// <summary>
        /// Register a new user to the system
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterRequest registerRequest)
        {
            var response = await _identityService.RegisterAsync(registerRequest);
            return Ok(response);
        }
    }
}
