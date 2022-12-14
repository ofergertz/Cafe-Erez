using Microsoft.AspNetCore.Identity;

namespace CafeErez.Shared.Model.Identity
{
    public class User : IdentityUser<string>
    {
        public override string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePictureDataUrl { get; set; }
    }
}
