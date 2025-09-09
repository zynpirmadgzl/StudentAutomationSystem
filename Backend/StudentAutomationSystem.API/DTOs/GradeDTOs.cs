using System.ComponentModel.DataAnnotations;

namespace StudentAutomationSystem.API.DTOs
{
    public class GradeDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public string GradeType { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
    
    public class CreateGradeDto
    {
        [Required]
        public int StudentId { get; set; }
        
        [Required]
        public int CourseId { get; set; }
        
        [Range(0, 100)]
        public decimal Score { get; set; }
        
        [Required]
        public string GradeType { get; set; } = "Exam";
        
        public string Comments { get; set; } = string.Empty;
    }
}