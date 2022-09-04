using CafeErez.Shared.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.BusinessService.Authentication
{
    public interface IAuthenticationManager
    {
        Task<IServiceWrapper<LoginResponse>> Login(LoginRequest loginRequest);
        Task<IServiceWrapper> Register(RegisterRequest registerRequest);
        Task<IServiceWrapper> Logout();

    }
}
