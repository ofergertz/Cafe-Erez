using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CafeErez.Shared.Constants.Constants;

namespace BusinessService
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUsersHandler _usersHandler;

        public UserService(UserManager<User> userManager, IUsersHandler usersHandler)
        {
            _userManager = userManager;
            _usersHandler = usersHandler;
        }

        public async Task<IServiceWrapper<List<UserResponse>>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = await _usersHandler.MapUserFromAdminUsersRequest(users);
            return await ServiceWrapper<List<UserResponse>>.SuccessAsync(result);
        }

        public async Task<IServiceWrapper<UserResponse>> GetUserAsync(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
            var result = await _usersHandler.MapUserToDto(user);
            return await ServiceWrapper<UserResponse>.SuccessAsync(result);
        }
    }
}
