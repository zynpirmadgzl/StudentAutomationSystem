using System.ComponentModel.DataAnnotations;

namespace StudentAutomationSystem.API.Models
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
        
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        
        public DateTime Date { get; set; }
        
        public bool IsPresent { get; set; }
        
        [MaxLength(200)]
        public string Notes { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}