using System.ComponentModel.DataAnnotations;

namespace StudentAutomationSystem.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Şifre gereklidir")]
        public string Password { get; set; } = string.Empty;
    }
    
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Şifre gereklidir")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Ad gereklidir")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Soyad gereklidir")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Rol seçimi gereklidir")]
        public UserRole Role { get; set; }
        
        public string? StudentNumber { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? Department { get; set; }
    }
    
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}