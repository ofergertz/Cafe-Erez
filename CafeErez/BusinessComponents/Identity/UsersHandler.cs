using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessComponents.Identity
{
    public class UsersHandler : IUsersHandler
    {
        private readonly IServiceProvider _serviceProvider;
        public UsersHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<User> MapUserFromRequest(RegisterRequest registerRequest)
        {
            var mapper = (IMapper<RegisterRequest, User>)_serviceProvider.GetService(typeof(IMapper<RegisterRequest, User>));
            return mapper.Map(registerRequest);
        }
    }
}
