using Microsoft.AspNetCore.Identity;

namespace CafeErez.Shared.BusinessComponents
{
    public class AppRole : IdentityRole
    {
        public string Description { get; set; }

        public AppRole() : base() { }
    }
}
