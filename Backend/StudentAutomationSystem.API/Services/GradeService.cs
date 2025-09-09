using Microsoft.EntityFrameworkCore;
using StudentAutomationSystem.API.Data;
using StudentAutomationSystem.API.DTOs;
using StudentAutomationSystem.API.Models;

namespace StudentAutomationSystem.API.Services
{
    public class GradeService : IGradeService
    {
        private readonly ApplicationDbContext _context;

        public GradeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GradeDto>> GetGradesByStudentIdAsync(int studentId)
        {
            return await _context.Grades
                .Include(g => g.Course)
                .Where(g => g.StudentId == studentId)
                .Select(g => new GradeDto
                {
                    Id = g.Id,
                    StudentId = g.StudentId,
                    CourseId = g.CourseId,
                    CourseName = g.Course.CourseName,
                    Score = g.Score,
                    GradeType = g.GradeType,
                    Comments = g.Comments,
                    CreatedAt = g.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<GradeDto?> AddGradeAsync(CreateGradeDto createGradeDto)
        {
            var grade = new Grade
            {
                StudentId = createGradeDto.StudentId,
                CourseId = createGradeDto.CourseId,
                Score = createGradeDto.Score,
                GradeType = createGradeDto.GradeType,
                Comments = createGradeDto.Comments
            };

            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();

            return new GradeDto
            {
                Id = grade.Id,
                StudentId = grade.StudentId,
                CourseId = grade.CourseId,
                Score = grade.Score,
                GradeType = grade.GradeType,
                Comments = grade.Comments,
                CreatedAt = grade.CreatedAt
            };
        }

        public async Task<bool> DeleteGradeAsync(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null) return false;

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
