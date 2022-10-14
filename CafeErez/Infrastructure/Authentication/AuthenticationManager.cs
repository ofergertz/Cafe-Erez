using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Authentication;
using CafeErez.Shared.Constants;
using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using System.Security.Claims;

namespace Infrastructure.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationManager(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<ClaimsPrincipal> CurrentUser()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User;
        }

        public async Task<IServiceWrapper<LoginResponse>> Login(LoginRequest loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync(Constants.IdentityAuthenticationApi.Login, loginRequest);
            var result = await response.ToResult<LoginResponse>();
            if (result.Succeeded)
            {
                await _localStorage.SetItemAsync("authToken", result.Data.Token);
                ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(result.Data.UserName);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Data.Token);
            }
            return result;
        }

        public async Task<IServiceWrapper> Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return await ServiceWrapper.SuccessAsync();
        }

        public async Task<IServiceWrapper<RegisterResponse>> Register(RegisterRequest registerRequest)
        {
            var response = await _httpClient.PostAsJsonAsync(Constants.IdentityAuthenticationApi.Register, registerRequest);
            var result = await response.ToResult<RegisterResponse>();
            return result;
        }
    }
}
