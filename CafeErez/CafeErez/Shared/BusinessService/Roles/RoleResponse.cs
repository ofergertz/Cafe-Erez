using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.BusinessService.Roles
{
    public class RoleResponse
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}
