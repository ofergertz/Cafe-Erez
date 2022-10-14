using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.Model.Infrastructure
{
    public interface IRoleManager
    {
        Task<IServiceWrapper<List<RoleResponse>>> GetRolesAsync();

        Task<IServiceWrapper<string>> SaveAsync(RoleRequest role);

        Task<IServiceWrapper<string>> DeleteAsync(string id);

        //Task<IServiceWrapper<PermissionResponse>> GetPermissionsAsync(string roleId);

        //Task<IServiceWrapper<string>> UpdatePermissionsAsync(PermissionRequest request);
    }
}
