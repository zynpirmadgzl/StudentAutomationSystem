using System.ComponentModel.DataAnnotations;

namespace StudentAutomationSystem.API.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string CourseCode { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string CourseName { get; set; } = string.Empty;
        
        [Range(1, 10)]
        public int Credits { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;
        
        public CourseStatus Status { get; set; } = CourseStatus.NotStarted;
        
        // Navigation Properties
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
    
    public enum CourseStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2,
        Cancelled = 3
    }
}