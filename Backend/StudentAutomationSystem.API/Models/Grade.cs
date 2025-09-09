using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAutomationSystem.API.Models
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }
        
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
        
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        
        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 100)]
        public decimal Score { get; set; }
        
        [MaxLength(20)]
        public string GradeType { get; set; } = "Exam"; // Exam, Quiz, Assignment, etc.
        
        [MaxLength(500)]
        public string Comments { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}