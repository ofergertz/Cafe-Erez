using CafeErez.Shared.BusinessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.Model.Identity
{
    public interface IApplicationUserManager
    {
        Task<IServiceWrapper<List<UserResponse>>> GetAllUsersAsync();
    }
}
