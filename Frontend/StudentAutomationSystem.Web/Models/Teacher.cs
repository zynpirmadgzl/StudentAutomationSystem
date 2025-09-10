using System.ComponentModel.DataAnnotations;

namespace StudentAutomationSystem.Web.Models
{
    public class TeacherModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
        
        public string FullName => $"{FirstName} {LastName}";
    }
    
    public class CreateTeacher
    {
        [Required(ErrorMessage = "Email gereklidir")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Ad gereklidir")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Soyad gereklidir")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Personel numarası gereklidir")]
        public string EmployeeNumber { get; set; } = string.Empty;
        
        public string Department { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Şifre gereklidir")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Password { get; set; } = string.Empty;
    }
}