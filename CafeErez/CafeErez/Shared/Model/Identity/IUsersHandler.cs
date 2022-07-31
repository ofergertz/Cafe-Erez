using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.Model.Identity
{
    public interface IUsersHandler
    {
        Task<User> MapUserFromRequest(RegisterRequest registerRequest);

    }
}
