using System.Net.Http.Json;
using StudentAutomationSystem.Web.Models;

namespace StudentAutomationSystem.Web.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>?> GetAllStudentsAsync();
    }

    public class StudentService : IStudentService
    {
        private readonly HttpClient _http;

        public StudentService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<Student>?> GetAllStudentsAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<Student>>("api/students");
        }
    }
}
