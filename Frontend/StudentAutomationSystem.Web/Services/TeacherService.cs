using StudentAutomationSystem.Web.Models;

public class TeacherService : ITeacherService
{
    public Task<List<Teacher>> GetAllTeachersAsync()
        => Task.FromResult(new List<Teacher>());
}
