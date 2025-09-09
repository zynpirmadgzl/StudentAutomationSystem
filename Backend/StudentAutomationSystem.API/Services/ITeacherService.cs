using StudentAutomationSystem.API.DTOs;

namespace StudentAutomationSystem.API.Services
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherDto>> GetAllTeachersAsync();
        Task<TeacherDto?> GetTeacherByIdAsync(int id);
        Task<TeacherDto?> CreateTeacherAsync(CreateTeacherDto createTeacherDto);
        Task<TeacherDto?> UpdateTeacherAsync(int id, UpdateTeacherDto updateTeacherDto);
        Task<bool> DeleteTeacherAsync(int id);
    }
}
