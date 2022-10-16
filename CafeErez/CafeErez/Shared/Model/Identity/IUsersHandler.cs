using CafeErez.Shared.BusinessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.Model.Identity
{
    public interface IUsersHandler
    {
        Task<User> MapUserFromRegisterRequest(RegisterRequest registerRequest);
        Task<List<UserResponse>> MapUserFromAdminUsersRequest(List<User> users);
        Task<UserResponse> MapUserToDto(User user);

    }
}
