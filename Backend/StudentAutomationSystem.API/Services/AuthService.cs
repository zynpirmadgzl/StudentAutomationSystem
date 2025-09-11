using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        public async Task LogoutAsync()
{
    await _signInManager.SignOutAsync();

}

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return null;

            var token = GenerateJwtToken(user.Id, user.Email ?? "", user.Role.ToString());

            return new AuthResponseDto
            {
                UserId = user.Id,
                Token = token,
                Email = user.Email ?? "",
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["JwtSettings:ExpirationInDays"]))
            };
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
        {
            // Transaction başlat
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
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
                    Role = registerDto.Role,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                // Role'e göre ek tablo kayıtları
                if (registerDto.Role == UserRole.Student && !string.IsNullOrEmpty(registerDto.StudentNumber))
                {
                    // Öğrenci numarası kontrolü
                    var existingStudent = await _context.Students.AnyAsync(s => s.StudentNumber == registerDto.StudentNumber);
                    if (existingStudent)
                    {
                        await transaction.RollbackAsync();
                        return null;
                    }

                    var student = new Student
                    {
                        UserId = user.Id,
                        StudentNumber = registerDto.StudentNumber,
                        EnrollmentDate = DateTime.UtcNow
                    };
                    
                    _context.Students.Add(student);
                    Console.WriteLine($"Student kaydı eklendi: UserId={user.Id}, StudentNumber={registerDto.StudentNumber}");
                }
                else if (registerDto.Role == UserRole.Teacher && !string.IsNullOrEmpty(registerDto.EmployeeNumber))
                {
                    // Personel numarası kontrolü
                    var existingTeacher = await _context.Teachers.AnyAsync(t => t.EmployeeNumber == registerDto.EmployeeNumber);
                    if (existingTeacher)
                    {
                        await transaction.RollbackAsync();
                        return null;
                    }

                    var teacher = new Teacher
                    {
                        UserId = user.Id,
                        EmployeeNumber = registerDto.EmployeeNumber,
                        Department = registerDto.Department ?? string.Empty
                    };
                    
                    _context.Teachers.Add(teacher);
                    Console.WriteLine($"Teacher kaydı eklendi: UserId={user.Id}, EmployeeNumber={registerDto.EmployeeNumber}");
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var token = GenerateJwtToken(user.Id, user.Email ?? "", user.Role.ToString());

                Console.WriteLine($"Kayıt başarılı: UserId={user.Id}, Role={user.Role}");

                return new AuthResponseDto
                {
                    UserId = user.Id,
                    Token = token,
                    Email = user.Email ?? "",
                    FirstName = user.FirstName ?? "",
                    LastName = user.LastName ?? "",
                    Role = user.Role,
                    ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["JwtSettings:ExpirationInDays"]))
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kayıt sırasında hata: {ex.Message}");
                await transaction.RollbackAsync();
                return null;
            }
        }

        public string GenerateJwtToken(string userId, string email, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? throw new ArgumentNullException("JwtSettings:SecretKey"));

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
                Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(jwtSettings["ExpirationInDays"] ?? "7")),
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