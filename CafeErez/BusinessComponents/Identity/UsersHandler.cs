using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Constants;
using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static CafeErez.Shared.Constants.Constants;

namespace BusinessComponents.Identity
{
    public class UsersHandler : IUsersHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public UsersHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<User> MapUserFromRegisterRequest(RegisterRequest registerRequest)
        {
            var mapper = (IMapper<RegisterRequest, User>)_serviceProvider.GetService(typeof(IMapper<RegisterRequest, User>));
            return mapper.Map(registerRequest);
        }

        public async Task<List<UserResponse>> MapUserFromAdminUsersRequest(List<User> users)
        {
            var mapper = (IMapper<List<User>, List<UserResponse>>)_serviceProvider.GetService(typeof(IMapper<RegisterRequest, User>));
            return mapper.Map(users);
        }

        public async Task<UserResponse> MapUserToDto(User user)
        {
            var mapper = (IMapper<User, UserResponse>)_serviceProvider.GetService(typeof(IMapper<User, UserResponse>));
            return mapper.Map(user);
        }
    }
}
