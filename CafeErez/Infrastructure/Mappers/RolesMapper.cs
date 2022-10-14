using CafeErez.Shared.BusinessComponents;
using CafeErez.Shared.BusinessService.Roles;
using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mappers
{
    public class RolesMapper : IMapper<List<AppRole>, List<RolesResponse>>,
                               IMapper<RoleRequest, AppRole>             
    {
        public List<RolesResponse> Map(List<AppRole> roleClaims)
        {
            List<RolesResponse> roleClaimResponses = new List<RolesResponse>();
            foreach (var roleClaim in roleClaims)
            {
                roleClaimResponses.Add(new RolesResponse()
                {
                    Description = roleClaim.Description,
                    Id = roleClaim.Id,
                    RoleName = roleClaim.Name,                    
                });
            }

            return roleClaimResponses;
        }

        public AppRole Map(RoleRequest source)
        {
            return new AppRole()
            {
                Description = source.Description,
                Name = source.Name
            };
        }
    }
}
