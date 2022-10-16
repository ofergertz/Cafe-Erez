using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.Model.Users
{
    public interface IUserManager
    {
        Task<IServiceWrapper<List<UserResponse>>> GetAllUsersAsync();
        Task<IServiceWrapper<UserResponse>> GetUserAsync(string userId);
    }
}
