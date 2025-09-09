using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StudentAutomationSystem.API.Data;
using StudentAutomationSystem.API.DTOs;
using StudentAutomationSystem.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentAutomationSystem.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        
        public AuthService(
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }
        
        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !user.IsActive)
                return null;
                
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return null;
                
            var token = GenerateJwtToken(user.Id, user.Email, user.Role.ToString());
            
            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
        }
        
        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
                return null;
                
            var user = new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Role = registerDto.Role
            };
            
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return null;
                
            // Role'e göre ek tablo kayıtları
            if (registerDto.Role == UserRole.Student && !string.IsNullOrEmpty(registerDto.StudentNumber))
            {
                var student = new Student
                {
                    UserId = user.Id,
                    StudentNumber = registerDto.StudentNumber
                };
                _context.Students.Add(student);
            }
            else if (registerDto.Role == UserRole.Teacher && !string.IsNullOrEmpty(registerDto.EmployeeNumber))
            {
                var teacher = new Teacher
                {
                    UserId = user.Id,
                    EmployeeNumber = registerDto.EmployeeNumber,
                    Department = registerDto.Department ?? string.Empty
                };
                _context.Teachers.Add(teacher);
            }
            
            await _context.SaveChangesAsync();
            
            var token = GenerateJwtToken(user.Id, user.Email, user.Role.ToString());
            
            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
        }
        
        public string GenerateJwtToken(string userId, string email, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(jwtSettings["ExpirationInDays"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}