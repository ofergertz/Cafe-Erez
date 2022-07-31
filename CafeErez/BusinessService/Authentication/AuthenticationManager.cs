using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Authentication;
using CafeErez.Shared.Constants;
using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Identity;
using System.Net.Http.Json;

namespace BusinessService.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly HttpClient _httpClient;

        public AuthenticationManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IServiceWrapper> Login(LoginRequest loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync(Constants.IdentityAuthenticationApi.Login, loginRequest);
            var result = await response.ToResult<LoginResponse>();

            return result;
        }

        public Task<IServiceWrapper> Logout()
        {
            throw new NotImplementedException();
        }

        public Task<IServiceWrapper> Register(RegisterRequest registerRequest)
        {
            throw new NotImplementedException();
        }
    }
}
