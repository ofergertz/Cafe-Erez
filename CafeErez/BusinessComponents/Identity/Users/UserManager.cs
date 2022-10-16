using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Constants;
using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Identity;
using CafeErez.Shared.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessComponents.Identity.Users
{
    public class UserManager : IUserManager
    {
        private readonly HttpClient _httpClient;

        public UserManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IServiceWrapper<UserResponse>> GetUserAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"{Constants.Users.GetUser}/{userId}");
            return await response.ToResult<UserResponse>();
        }

        public async Task<IServiceWrapper<List<UserResponse>>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync(Constants.Users.GetAllUsers);
            return await response.ToResult<List<UserResponse>>();
        }
    }
}
