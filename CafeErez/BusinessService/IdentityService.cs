using CafeErez.Shared.BusinessComponents;
using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Constants;
using CafeErez.Shared.Model.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BusinessService
{
    public class IdentityService : IIdentityService
    {
        private readonly IStringLocalizer<IdentityService> _localizer;
        private readonly IServiceProvider _serviceProvider;
        //private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUsersHandler _userHandler;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<AppRole> _roleManagers;

        public IdentityService(IStringLocalizer<IdentityService> localizer,
            IServiceProvider serviceProvider,
            UserManager<User> userManagers, RoleManager<AppRole> roleManagers, 
            IUsersHandler userHandler)
        {
            _localizer = localizer;
            _serviceProvider = serviceProvider;
            _userManager = userManagers;
            _roleManagers = roleManagers;
            _userHandler = userHandler;
        }

        public async Task<IServiceWrapper<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null)
            {
                return await ServiceWrapper<LoginResponse>.FailAsync(_localizer["User Not Found."]);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!passwordValid)
            {
                return await ServiceWrapper<LoginResponse>.FailAsync(_localizer["Invalid Credentials."]);
            }

            var response = CreateLoginResponse(user);
            return await ServiceWrapper<LoginResponse>.SuccessAsync(response);
        }

        public async Task<IServiceWrapper<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest)
        {
            //take out to validator
            var userWithSameUserName = await _userManager.FindByNameAsync(registerRequest.UserName);
            if (userWithSameUserName != null)
            {
                return await ServiceWrapper<RegisterResponse>.FailAsync(string.Format(_localizer["Username {0} is already taken."], registerRequest.UserName));
            }

            var user = await _userHandler.MapUserFromRequest(registerRequest);

            if (!string.IsNullOrWhiteSpace(registerRequest.PhoneNumber))
            {
                var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == registerRequest.PhoneNumber);
                if (userWithSamePhoneNumber != null)
                {
                    return await ServiceWrapper<RegisterResponse>.FailAsync(string.Format(_localizer["Phone number {0} is already registered."], registerRequest.PhoneNumber));
                }
            }

            var identityUser = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!identityUser.Succeeded)
                return await ServiceWrapper<RegisterResponse>.FailAsync(identityUser.Errors.Select(a => _localizer[a.Description].ToString()).ToList());

            await _userManager.AddToRoleAsync(user, Constants.Roles.BasicRole);
            return await ServiceWrapper<RegisterResponse>.SuccessAsync(CreateRegisterResponse(user));
        }
        private LoginResponse CreateLoginResponse(User user)
        {
            var response = new LoginResponse();
            response.UserImageURL = user.ProfilePictureDataUrl;
            response.UserName = user.UserName;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;

            return response;
        }
        private RegisterResponse CreateRegisterResponse(User user)
        {
            var serviceResponse = new RegisterResponse();
            serviceResponse.UserName = user.UserName;
            serviceResponse.LastName = user.LastName;
            serviceResponse.FirstName = user.FirstName;
            return serviceResponse;
        }
    }
}