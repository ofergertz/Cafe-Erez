using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace Infrastructure.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        /// Asynchronously gets an <see cref="AuthenticationState"/> that describes the current user
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public CustomAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(
                    new ClaimsIdentity()));
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            /// A Claim is a statement about an entity by an Issuer.
            /// A Claim consists of a Type, Value, a Subject and an Issuer.
            /// An Identity that is represented by a set of claims.
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt");
            //supporting multiple claims-based identities
            var user = new ClaimsPrincipal(identity);
            /// Provides information about the currently authenticated user, if any.
            var state = new AuthenticationState(user);
            var authState = Task.FromResult(state);
            NotifyAuthenticationStateChanged(authState);
            return state;
        }
        public void MarkUserAsAuthenticated(string UserName)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, UserName) }, "apiauth"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }
        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            keyValuePairs.TryGetValue(ClaimTypes.Name, out object UserName);
            keyValuePairs.TryGetValue(ClaimTypes.NameIdentifier, out object UserName1);
            keyValuePairs.TryGetValue(ClaimTypes.Email, out object UserName2);
            keyValuePairs.TryGetValue(ClaimTypes.Surname, out object UserName3);
            keyValuePairs.TryGetValue("unique_name", out object UserName4);
            if (UserName4 != null)
            {
                if (UserName4.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(UserName4.ToString());
                    claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName4.ToString()));
                }
                keyValuePairs.Remove(ClaimTypes.Name);
            }
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }
        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
