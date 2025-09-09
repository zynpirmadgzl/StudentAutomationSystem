using StudentAutomationSystem.API.DTOs;

namespace StudentAutomationSystem.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
        string GenerateJwtToken(string userId, string email, string role);
    }
}