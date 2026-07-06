using UniversityDataPlatform.Models;
using UniversityDataPlatform.Repositories.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using UniversityDataPlatform.Services; // CustomAuthStateProvider burada olduğu için ekledik

namespace UniversityDataPlatform.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(
            IUserRepository userRepository,
            AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage)
        {
            _userRepository = userRepository;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            // 1. Fetch user from database
            var user = await _userRepository.GetByEmailAsync(email);

            // 2. If user exists and password is correct (Verify using PasswordHasher)
            if (user != null)
            {
                var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, user.Password, password);
                if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
                {
                    // 3. Fill the Session object with actual database values
                    var session = new UserSession
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        Role = user.Role, // Critical: Database 'Admin' or 'Staff' role is loaded here
                        FacultyId = user.FacultyId
                    };

                    // 4. Cast the Provider and update state
                    var customProvider = (CustomAuthStateProvider)_authStateProvider;
                    await customProvider.UpdateAuthenticationState(session);

                    return user;
                }
            }
            return null;
        }

        public async Task LogoutAsync()
        {
            // Pass null to sign out;
            // UpdateAuthenticationState will automatically clear LocalStorage.
            var customProvider = (CustomAuthStateProvider)_authStateProvider;
            await customProvider.UpdateAuthenticationState(null);
        }
    }
}