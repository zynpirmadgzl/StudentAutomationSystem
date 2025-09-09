using System.Net.Http.Json;
using StudentAutomationSystem.Web.Models;

namespace StudentAutomationSystem.Web.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(Login dto);
        Task<AuthResponse?> RegisterAsync(Register dto);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<AuthResponse?> LoginAsync(Login dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        }

        public async Task<AuthResponse?> RegisterAsync(Register dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", dto);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }
    }
}
