using CafeErez.Shared.BusinessComponents;
using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Constants;
using CafeErez.Shared.Model.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Net.Sockets;

namespace BusinessService
{
    public class IdentityService : IIdentityService
    {
        private readonly IStringLocalizer<IdentityService> _localizer;
        private readonly IServiceProvider _serviceProvider;
        //private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUsersHandler _userHandler;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;

        public IdentityService(IStringLocalizer<IdentityService> localizer,
            IServiceProvider serviceProvider,
            UserManager<User> userManagers, RoleManager<AppRole> roleManager,
            IUsersHandler userHandler, IConfiguration configuration)
        {
            _localizer = localizer;
            _serviceProvider = serviceProvider;
            _userManager = userManagers;
            _roleManager = roleManager;
            _userHandler = userHandler;
            _configuration = configuration;
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

            var response = CreateLoginResponse(user, loginRequest);
            response.Token = await GenerateJwtAsync(user);
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
        private LoginResponse CreateLoginResponse(User user, LoginRequest loginRequest)
        {
            var response = new LoginResponse();
            response.UserImageURL = user.ProfilePictureDataUrl;
            response.UserName = user.UserName;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;

            return response;
        }

        private async Task<string> GenerateJwtAsync(User user)
        {
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            return token;
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            var permissionClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
                var thisRole = await _roleManager.FindByNameAsync(role);
                var allPermissionsForThisRoles = await _roleManager.GetClaimsAsync(thisRole);
                permissionClaims.AddRange(allPermissionsForThisRoles);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            }
            .Union(userClaims)
            .Union(roleClaims)
            .Union(permissionClaims);

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddDays(2),
               signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }

        //public string GenerateToken(User user)
        //{
        //    var mySecurityKey = _configuration["JwtSecurityKey"];
        //    var secret = Encoding.UTF8.GetBytes(mySecurityKey);

        //    /// A <see cref="SecurityTokenHandler"/> designed for creating and validating Json Web Tokens.
        //    //var tokenHandler = new JwtSecurityTokenHandler();
        //    ///// Contains some information which used to create a security token.
        //    //var tokenDescriptor = new SecurityTokenDescriptor
        //    //{
        //    //    /// Gets or sets the <see cref="ClaimsIdentity"/>.
        //    //    /// If both <cref see="Claims"/> and <see cref="Subject"/> are set, the claim values in Subject will be combined with the values
        //    //    /// in Claims. The values found in Claims take precedence over those found in Subject, so any duplicate
        //    //    /// values will be overridden.
        //    //    Subject = new ClaimsIdentity(new Claim[]
        //    //    {
        //    //         new Claim(ClaimTypes.Name, user.UserName),
        //    //         new Claim(ClaimTypes.NameIdentifier, user.Id),
        //    //         new Claim(ClaimTypes.Email, user.Email),
        //    //         new Claim(ClaimTypes.GivenName, user.FirstName),
        //    //         new Claim(ClaimTypes.Surname, user.LastName)
        //    //    }),
        //    //    Expires = DateTime.UtcNow.AddDays(1),
        //    //    Issuer = _configuration["JwtIssuer"],
        //    //    Audience = _configuration["JwtAudience"],
        //    //    SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
        //    //};
        //    ///// Creates a Json Web Token (JWT).
        //    //var token = tokenHandler.CreateToken(tokenDescriptor);
        //    ///// Serializes a <see cref="JwtSecurityToken"/> into a JWT in Compact Serialization Format.
        //    ///
        //    var claims = new List<Claim>
        //    {
        //        new(ClaimTypes.NameIdentifier, user.Id),
        //        new(ClaimTypes.Email, user.Email),
        //        new(ClaimTypes.Name, user.FirstName),
        //        new(ClaimTypes.Surname, user.LastName),
        //        new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
        //    };
        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddDays(1),
        //       signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256)
        //        );
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var encryptedToken = tokenHandler.WriteToken(token);
        //    return encryptedToken;
        //}

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