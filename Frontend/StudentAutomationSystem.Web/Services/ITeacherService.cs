using StudentAutomationSystem.Web.Models;
namespace StudentAutomationSystem.Web.Services
{
    public interface ITeacherService
    {
        Task<List<Teacher>> GetTeachersAsync();
    }
}