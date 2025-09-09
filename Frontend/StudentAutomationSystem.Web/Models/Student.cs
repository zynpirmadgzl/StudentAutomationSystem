using System.ComponentModel.DataAnnotations;

namespace StudentAutomationSystem.Web.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string StudentNumber { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public bool IsActive { get; set; }
        
        public string FullName => $"{FirstName} {LastName}";
    }
    
    public class CreateStudent
    {
        [Required(ErrorMessage = "Email gereklidir")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Ad gereklidir")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Soyad gereklidir")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Öğrenci numarası gereklidir")]
        public string StudentNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Şifre gereklidir")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Password { get; set; } = string.Empty;
    }
    
    public class UpdateStudent
    {
        [Required(ErrorMessage = "Ad gereklidir")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Soyad gereklidir")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email gereklidir")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}