using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using StudentAutomationSystem.Web.Models;
using System.Security.Claims;

namespace StudentAutomationSystem.Web.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ILogger<CustomAuthStateProvider> _logger;
        
        public CustomAuthStateProvider(ILocalStorageService localStorage, ILogger<CustomAuthStateProvider> logger)
        {
            _localStorage = localStorage;
            _logger = logger;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var currentUser = await _localStorage.GetItemAsync<AuthResponse>("currentUser");
                
                if (currentUser != null && !string.IsNullOrEmpty(currentUser.Token) && 
                    currentUser.ExpiresAt > DateTime.UtcNow)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, currentUser.Email),
                        new Claim(ClaimTypes.Email, currentUser.Email),
                        new Claim(ClaimTypes.Name, $"{currentUser.FirstName} {currentUser.LastName}"),
                        new Claim(ClaimTypes.Role, currentUser.Role.ToString()),
                        new Claim("FirstName", currentUser.FirstName),
                        new Claim("LastName", currentUser.LastName),
                        new Claim("Role", currentUser.Role.ToString())
                    };
                    
                    var identity = new ClaimsIdentity(claims, "Bearer");
                    var user = new ClaimsPrincipal(identity);
                    
                    return new AuthenticationState(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting authentication state");
            }
            
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        
        public async Task UpdateAuthenticationState(AuthResponse? authResponse)
        {
            ClaimsPrincipal user;
            
            if (authResponse != null && !string.IsNullOrEmpty(authResponse.Token))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, authResponse.Email),
                    new Claim(ClaimTypes.Email, authResponse.Email),
                    new Claim(ClaimTypes.Name, $"{authResponse.FirstName} {authResponse.LastName}"),
                    new Claim(ClaimTypes.Role, authResponse.Role.ToString()),
                    new Claim("FirstName", authResponse.FirstName),
                    new Claim("LastName", authResponse.LastName),
                    new Claim("Role", authResponse.Role.ToString())
                };
                
                var identity = new ClaimsIdentity(claims, "Bearer");
                user = new ClaimsPrincipal(identity);
            }
            else
            {
                user = new ClaimsPrincipal(new ClaimsIdentity());
            }
            
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}