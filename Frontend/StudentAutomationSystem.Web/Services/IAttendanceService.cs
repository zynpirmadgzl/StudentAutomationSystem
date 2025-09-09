using StudentAutomationSystem.Web.Models;

namespace StudentAutomationSystem.Web.Services
{
    public interface IAttendanceService
    {
        Task<List<Attendance>> GetAttendancesAsync();
    }
}