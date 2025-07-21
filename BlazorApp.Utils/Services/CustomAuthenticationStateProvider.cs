using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorApp.Utils.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string? token = await _localStorageService.GetItemAsync<string>("AuthToken");

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            IEnumerable<Claim> claims = ParseClaimsFromJwt(token);
            ClaimsIdentity identity = new(claims, "jwt");
            ClaimsPrincipal user = new(identity);

            return new AuthenticationState(user);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            JwtSecurityToken token = _tokenHandler.ReadJwtToken(jwt);
            return token.Claims;
        }

        public void MarkUserAsAuthenticated(string token)
        {
            IEnumerable<Claim> claims = ParseClaimsFromJwt(token);
            ClaimsIdentity identity = new(claims, "jwt");
            ClaimsPrincipal user = new(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void MarkUserAsLoggedOut()
        {
            ClaimsPrincipal? anonymous = new(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }
    }
}