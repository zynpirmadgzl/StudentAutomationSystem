using System.ComponentModel.DataAnnotations;

namespace StudentAutomationSystem.API.Models
{
    public class StudentCourse
    {
        [Key]
        public int Id { get; set; }
        
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
        
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
    }
}