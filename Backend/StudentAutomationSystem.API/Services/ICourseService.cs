using StudentAutomationSystem.API.DTOs;

namespace StudentAutomationSystem.API.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto?> GetCourseByIdAsync(int id);
        Task<CourseDto?> CreateCourseAsync(CreateCourseDto createCourseDto);
        Task<bool> AddStudentToCourseAsync(int courseId, int studentId);
        Task<bool> RemoveStudentFromCourseAsync(int courseId, int studentId);
    }
}
