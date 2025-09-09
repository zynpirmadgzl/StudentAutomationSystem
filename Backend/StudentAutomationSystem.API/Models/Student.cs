using System.ComponentModel.DataAnnotations;
namespace StudentAutomationSystem.API.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        public User User { get; set; } = null!;
        
        [Required]
        [MaxLength(20)]
        public string StudentNumber { get; set; } = string.Empty;
        
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        
        // Navigation Properties
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}