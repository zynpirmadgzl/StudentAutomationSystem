using Microsoft.AspNetCore.Mvc;
using StudentAutomationSystem.API.DTOs;
using StudentAutomationSystem.API.Services;

namespace StudentAutomationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            if (result == null)
                return Unauthorized("Invalid email or password");
                
            return Ok(result);
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            if (result == null)
                return BadRequest("User already exists or registration failed");
                
            return Ok(result);
        }
    }
}