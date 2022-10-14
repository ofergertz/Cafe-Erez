using CafeErez.Shared.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.BusinessService.Authentication
{
    public interface IAuthenticationManager
    {
        Task<IServiceWrapper<LoginResponse>> Login(LoginRequest loginRequest);
        Task<IServiceWrapper<RegisterResponse>> Register(RegisterRequest registerRequest);
        Task<IServiceWrapper> Logout();
        Task<ClaimsPrincipal> CurrentUser();

    }
}
