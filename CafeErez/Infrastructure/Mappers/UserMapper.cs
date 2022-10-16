using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CafeErez.Shared.Constants.Constants;

namespace Infrastructure.Mappers
{
    public class UserMapper : IMapper<RegisterRequest, User>,
                              IMapper<List<User>,List<UserResponse>>,
                              IMapper<User, UserResponse>

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

        public List<UserResponse> Map(List<User> source)
        {
            var usersResponse = new List<UserResponse>();
            foreach (var user in source)
            {
                var userResponse = Map(user);
                usersResponse.Add(userResponse);
            }

            return usersResponse;
        }

        public UserResponse Map(User user)
        {
            var userResponse = new UserResponse();
            userResponse.Id = user.Id;
            userResponse.FirstName = user.FirstName;
            userResponse.LastName = user.LastName;
            userResponse.UserName = user.UserName;
            userResponse.Email = user.Email;
            userResponse.EmailConfirmed = user.EmailConfirmed;
            userResponse.PhoneNumber = user.PhoneNumber;
            userResponse.ProfilePictureData = user.ProfilePictureDataUrl;

            return userResponse;
        }
    }
}
