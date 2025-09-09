using StudentAutomationSystem.API.DTOs;

namespace StudentAutomationSystem.API.Services
{
    public interface IGradeService
    {
        Task<IEnumerable<GradeDto>> GetGradesByStudentIdAsync(int studentId);
        Task<GradeDto?> AddGradeAsync(CreateGradeDto createGradeDto);
        Task<bool> DeleteGradeAsync(int id);
    }
}
