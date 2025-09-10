using StudentAutomationSystem.Web.Models;

namespace StudentAutomationSystem.Web.Services
{
    public interface ITeacherService
    {
        Task<List<TeacherModel>> GetAllTeachersAsync();
    }
}
