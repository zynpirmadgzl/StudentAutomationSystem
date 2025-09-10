using System.ComponentModel.DataAnnotations;

namespace StudentAutomationSystem.Web.Models
{
    public class CourseModel
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
    
    public class CreateCourse
    {
        [Required(ErrorMessage = "Ders kodu gereklidir")]
        [MaxLength(10)]
        public string CourseCode { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Ders adı gereklidir")]
        [MaxLength(100)]
        public string CourseName { get; set; } = string.Empty;
        
        [Range(1, 10, ErrorMessage = "Kredi 1-10 arası olmalıdır")]
        public int Credits { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Öğretmen seçimi gereklidir")]
        public int TeacherId { get; set; }
    }
    
    public enum CourseStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2,
        Cancelled = 3
    }
}