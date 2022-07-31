using CafeErez.Shared.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.BusinessService
{
    public interface IIdentityService : IService
    {
        Task<IServiceWrapper<LoginResponse>> LoginAsync(LoginRequest loginModel);
        Task<IServiceWrapper<RegisterResponse>> RegisterAsync(RegisterRequest registerModel);
    }
}
