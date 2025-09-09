using StudentAutomationSystem.Web.Models;
public class AttendanceService : IAttendanceService
{
    public Task<List<Attendance>> GetMyAttendanceAsync()
        => Task.FromResult(new List<Attendance>());
}


