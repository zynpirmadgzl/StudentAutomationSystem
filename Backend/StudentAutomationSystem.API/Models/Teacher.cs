using System.ComponentModel.DataAnnotations;

namespace StudentAutomationSystem.API.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        public User User { get; set; } = null!;
        
        [Required]
        [MaxLength(20)]
        public string EmployeeNumber { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;
        
        public DateTime HireDate { get; set; } = DateTime.UtcNow;
        
        // Navigation Properties
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}