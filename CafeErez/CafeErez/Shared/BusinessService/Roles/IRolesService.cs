using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.BusinessService.Roles
{
    public interface IRolesService
    {
        Task<ServiceWrapper<List<RolesResponse>>> GetAllAsync();
        Task<ServiceWrapper<string>> SaveAsync(RoleRequest request);
        Task<ServiceWrapper<string>> DeleteAsync(string id);

    }
}
