using System.Net.Http.Json;
using StudentAutomationSystem.Web.Models;

namespace StudentAutomationSystem.Web.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly HttpClient _httpClient;

        public TeacherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TeacherModel>> GetAllTeachersAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<TeacherModel>>("api/teachers");
            return result ?? new List<TeacherModel>();
        }
    }
}
