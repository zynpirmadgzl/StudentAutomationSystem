using StudentAutomationSystem.Web.Models;

namespace StudentAutomationSystem.Web.Services
{
    public interface IGradeService
    {
        Task<List<GradeModel>> GetAllGradesAsync();
        Task<List<GradeModel>> GetMyGradesAsync();
    }
}
