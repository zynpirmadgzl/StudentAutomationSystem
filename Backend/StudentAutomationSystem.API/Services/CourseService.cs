using Microsoft.EntityFrameworkCore;
using StudentAutomationSystem.API.Data;
using StudentAutomationSystem.API.DTOs;
using StudentAutomationSystem.API.Models;

namespace StudentAutomationSystem.API.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.Teacher).ThenInclude(t => t.User)
                .Include(c => c.Enrollments)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    CourseCode = c.CourseCode,
                    CourseName = c.CourseName,
                    Credits = c.Credits,
                    Description = c.Description,
                    TeacherId = c.TeacherId,
                    TeacherName = c.Teacher.User.FirstName + " " + c.Teacher.User.LastName,
                    Status = c.Status,
                    StudentCount = c.Enrollments.Count
                })
                .ToListAsync();
        }

        public async Task<CourseDto?> GetCourseByIdAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Teacher).ThenInclude(t => t.User)
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return null;

            return new CourseDto
            {
                Id = course.Id,
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                Credits = course.Credits,
                Description = course.Description,
                TeacherId = course.TeacherId,
                TeacherName = course.Teacher.User.FirstName + " " + course.Teacher.User.LastName,
                Status = course.Status,
                StudentCount = course.Enrollments.Count
            };
        }

        public async Task<CourseDto?> CreateCourseAsync(CreateCourseDto createCourseDto)
        {
            // aynı course code var mı kontrol
            if (await _context.Courses.AnyAsync(c => c.CourseCode == createCourseDto.CourseCode))
                return null;

            var course = new Course
            {
                CourseCode = createCourseDto.CourseCode,
                CourseName = createCourseDto.CourseName,
                Credits = createCourseDto.Credits,
                Description = createCourseDto.Description,
                TeacherId = createCourseDto.TeacherId,
                Status = CourseStatus.NotStarted
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return new CourseDto
            {
                Id = course.Id,
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                Credits = course.Credits,
                Description = course.Description,
                TeacherId = course.TeacherId,
                TeacherName = "", // mapping yapılabilir
                Status = course.Status,
                StudentCount = 0
            };
        }

        public async Task<bool> AddStudentToCourseAsync(int courseId, int studentId)
        {
            var course = await _context.Courses
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null) return false;

            if (course.Enrollments.Any(e => e.StudentId == studentId))
                return false; // zaten kayıtlı

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId
            };

            _context.Add(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveStudentFromCourseAsync(int courseId, int studentId)
        {
            var enrollment = await _context.Set<Enrollment>()
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == studentId);

            if (enrollment == null) return false;

            _context.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
