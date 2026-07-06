using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.LocalStorage;
using UniversityDataPlatform.Models;

namespace UniversityDataPlatform.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // CRITICAL: JS interop does not work during pre-rendering.
                // This try-catch block swallows the server-side "statically rendered" exception.
                var userSession = await _localStorage.GetItemAsync<UserSession>("UserSession");

                if (userSession == null)
                    return new AuthenticationState(_anonymous);

                return new AuthenticationState(CreateClaimsPrincipalFromSession(userSession));
            }
            catch (Exception)
            {
                // On error (if JS Interop is not ready yet), fall back to anonymous
                return new AuthenticationState(_anonymous);
            }
        }

        public async Task UpdateAuthenticationState(UserSession? userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if (userSession != null)
            {
                await _localStorage.SetItemAsync("UserSession", userSession);
                claimsPrincipal = CreateClaimsPrincipalFromSession(userSession);
            }
            else
            {
                await _localStorage.RemoveItemAsync("UserSession");
                claimsPrincipal = _anonymous;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

            private ClaimsPrincipal CreateClaimsPrincipalFromSession(UserSession session)
            {
                return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, session.Id.ToString()),
            new Claim(ClaimTypes.Name, session.FullName ?? ""),
            new Claim(ClaimTypes.Email, session.Email ?? ""),
            // Add the role as a claim to verify in the UI
            new Claim(ClaimTypes.Role, session.Role ?? "Guest"),
            new Claim("FacultyId", session.FacultyId.ToString())
        }, "CustomAuth"));


        }
    }
}