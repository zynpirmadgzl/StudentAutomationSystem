using System.Net.Http.Json;
using StudentAutomationSystem.Web.Models;

namespace StudentAutomationSystem.Web.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginModel dto);
        Task<AuthResponse?> RegisterAsync(RegisterModel dto);
        Task LogoutAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<AuthResponse?> LoginAsync(LoginModel dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterModel dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", dto);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }
        public async Task LogoutAsync()
    {
        // logout işlemleri (örneğin cookie, token temizleme)
        await Task.CompletedTask;
    }
    }
}
