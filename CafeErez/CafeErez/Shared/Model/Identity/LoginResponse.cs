using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.Model.Identity
{
    public class LoginResponse
    {
        public string UserName {get; set;}
        public string LastName {get; set;}
        public string FirstName {get; set;}
        public string UserImageURL { get; set; }
        public string Token { get; set; }

    }
}
