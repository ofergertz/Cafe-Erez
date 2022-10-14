using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Linq;
using static CafeErez.Shared.Constants.Constants;

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

            var claims = ParseClaimsFromJwt(savedToken);
            if (!IsValidToken(claims))
            {
                return new AuthenticationState(new ClaimsPrincipal(
                    new ClaimsIdentity()));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            /// A Claim is a statement about an entity by an Issuer.
            /// A Claim consists of a Type, Value, a Subject and an Issuer.
            /// An Identity that is represented by a set of claims.
            var identity = new ClaimsIdentity(claims, "jwt");
            //supporting multiple claims-based identities
            var user = new ClaimsPrincipal(identity);
            /// Provides information about the currently authenticated user, if any.
            var state = new AuthenticationState(user);
            var authState = Task.FromResult(state);
            NotifyAuthenticationStateChanged(authState);
            return state;
        }

        private bool IsValidToken(IEnumerable<Claim> claims)
        {
            var expDateFromClaim = claims.Where(claim => claim.Type.Equals("exp")).FirstOrDefault();
            if(expDateFromClaim != null)
            {
                var datetime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expDateFromClaim.Value));
                if (datetime.UtcDateTime > DateTime.UtcNow)
                    return true;
            }

            return false;
        }

        public void MarkUserAsAuthenticated(string UserName)
        {
            var authState = GetAuthenticationState(UserName);
            NotifyAuthenticationStateChanged(authState);
        }

        private async Task<AuthenticationState> GetAuthenticationState(string UserName)
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt")));

            return authState;
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

            if (keyValuePairs != null)
            {
                keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

                if (roles != null)
                {
                    if (roles.ToString().Trim().StartsWith("["))
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                        claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                    }

                    keyValuePairs.Remove(ClaimTypes.Role);
                }

                keyValuePairs.TryGetValue("exp", out var expiration);
                if (expiration.ToString().Trim().StartsWith("["))
                {
                    var parsedExpiration = JsonSerializer.Deserialize<string[]>(expiration.ToString());

                    claims.AddRange(parsedExpiration.Select(expiration => new Claim(ClaimTypes.Expiration, expiration)));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Expiration, expiration.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Expiration);

                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            }
            return claims;
            //var claims = new List<Claim>();
            //var payload = jwt.Split('.')[1];
            //var jsonBytes = ParseBase64WithoutPadding(payload);
            //var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            //keyValuePairs.TryGetValue(ClaimTypes.Name, out object UserName);
            //keyValuePairs.TryGetValue(ClaimTypes.NameIdentifier, out object UserName1);
            //keyValuePairs.TryGetValue(ClaimTypes.Email, out object UserName2);
            //keyValuePairs.TryGetValue(ClaimTypes.Surname, out object UserName3);
            //if (UserName != null)
            //{
            //    if (UserName.ToString().Trim().StartsWith("["))
            //    {
            //        var parsedRoles = JsonSerializer.Deserialize<string[]>(UserName.ToString());
            //        claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            //    }
            //    else
            //    {
            //        claims.Add(new Claim(ClaimTypes.Name, UserName.ToString()));
            //    }
            //    keyValuePairs.Remove(ClaimTypes.Name);
            //}
            //claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            //return claims;
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
