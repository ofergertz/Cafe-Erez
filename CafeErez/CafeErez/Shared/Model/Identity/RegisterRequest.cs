using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.Model.Identity
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(30, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "First Name must be filled")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name must be filled")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email Name must be filled ")]
        public string Email { get; set; }
        public string? ProfilePicture { get; set; }
        [Required(ErrorMessage = "Phone Number must be filled")]
        public string PhoneNumber { get; set; }
    }
}
