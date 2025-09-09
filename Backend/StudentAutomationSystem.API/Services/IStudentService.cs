using StudentAutomationSystem.API.DTOs;

namespace StudentAutomationSystem.API.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto?> GetStudentByIdAsync(int id);
        Task<StudentDto?> CreateStudentAsync(CreateStudentDto createStudentDto);
        Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentDto updateStudentDto);
        Task<bool> DeleteStudentAsync(int id);
        Task<IEnumerable<GradeDto>> GetStudentGradesAsync(int studentId);
    }
}
