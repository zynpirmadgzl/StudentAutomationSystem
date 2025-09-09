using System.ComponentModel.DataAnnotations;
using StudentAutomationSystem.API.Models;

namespace StudentAutomationSystem.API.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Description { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public CourseStatus Status { get; set; }
        public int StudentCount { get; set; }
    }
    
    public class CreateCourseDto
    {
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
        
        [Required]
        public int TeacherId { get; set; }
    }
}