using System.ComponentModel.DataAnnotations;

namespace StudentAutomationSystem.Web.Models
{
    public class GradeModel
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
        
        public string GradeLetter => CalculateGradeLetter(Score);
        
        private static string CalculateGradeLetter(decimal score)
        {
            return score switch
            {
                >= 90 => "AA",
                >= 85 => "BA",
                >= 80 => "BB",
                >= 75 => "CB",
                >= 70 => "CC",
                >= 65 => "DC",
                >= 60 => "DD",
                >= 50 => "FD",
                _ => "FF"
            };
        }
    }
    
    public class CreateGrade
    {
        [Required]
        public int StudentId { get; set; }
        
        [Required]
        public int CourseId { get; set; }
        
        [Range(0, 100, ErrorMessage = "Not 0-100 arası olmalıdır")]
        public decimal Score { get; set; }
        
        [Required]
        public string GradeType { get; set; } = "Sınav";
        
        public string Comments { get; set; } = string.Empty;
    }
}