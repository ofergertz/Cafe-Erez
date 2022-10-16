using CafeErez.Shared.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.BusinessService
{
    public interface IUserService
    {
        Task<IServiceWrapper<List<UserResponse>>> GetAllAsync();
        Task<IServiceWrapper<UserResponse>> GetUserAsync(string userID);
    }
}
