using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Constants;
using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class ApplicationUserManager : IApplicationUserManager
    {
        private readonly HttpClient _httpClient;

        public ApplicationUserManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IServiceWrapper<List<UserResponse>>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync(Constants.Users.GetAllUsers);
            return await response.ToResult<List<UserResponse>>();
        }
    }
}
