using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mappers
{
    public class UserMapper : IMapper<RegisterRequest, User>
    {
        public User Map(RegisterRequest registerRequest)
        {
            var user = new User();
            user.Id = Guid.NewGuid().ToString();
            user.FirstName = registerRequest.FirstName;
            user.LastName = registerRequest.LastName;
            user.PhoneNumber = registerRequest.PhoneNumber;
            user.ProfilePictureDataUrl = registerRequest.ProfilePicture;
            user.UserName = registerRequest.UserName;
            user.Email = registerRequest.Email;
            user.NormalizedEmail = registerRequest.Email.ToUpper();
            user.NormalizedUserName = registerRequest.UserName.ToUpper();
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
            return user;                 
        }
    }
}
