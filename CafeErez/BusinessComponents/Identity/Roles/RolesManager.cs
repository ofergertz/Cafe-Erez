using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Roles;
using CafeErez.Shared.Constants;
using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static CafeErez.Shared.Constants.Constants;

namespace BusinessComponents.Identity.Roles
{
    public class RolesManager : IRoleManager
    {
        private readonly HttpClient _httpClient;

        public RolesManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IServiceWrapper<List<RoleResponse>>> GetRolesAsync()
        {
            var response = await _httpClient.GetAsync(Constants.Roles.GetAll);
            return await response.ToResult<List<RoleResponse>>();
        }

        public async Task<IServiceWrapper<string>> SaveAsync(RoleRequest role)
        {
            var response = await _httpClient.PostAsJsonAsync(Constants.Roles.SaveRole, role);
            return await response.ToResult<string>();
        }

        public async Task<IServiceWrapper<string>> DeleteAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"{Constants.Roles.DeleteRole}/{id}");
            return await response.ToResult<string>();
        }
    }
}
