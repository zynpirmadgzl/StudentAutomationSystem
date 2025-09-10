using System.Net.Http.Json;
using StudentAutomationSystem.Web.Models;

namespace StudentAutomationSystem.Web.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseModel>?> GetAllCoursesAsync();
    }

    public class CourseService : ICourseService
    {
        private readonly HttpClient _http;

        public CourseService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<CourseModel>?> GetAllCoursesAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<CourseModel>>("api/courses");
        }
    }
}
