using StudentAutomationSystem.Web.Models;

namespace StudentAutomationSystem.Web.Services
{
    public interface IStudentService
    {
        Task<StudentModel> GetMyProfileAsync(); 
        Task<List<StudentModel>> GetAllStudentsAsync();
    }
}
