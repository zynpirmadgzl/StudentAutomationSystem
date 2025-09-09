using System.Net.Http.Json;
using StudentAutomationSystem.Web.Models;

namespace StudentAutomationSystem.Web.Services
{
    public class GradeService : IGradeService
    {
        private readonly HttpClient _httpClient;

        public GradeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Grade>> GetGradesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Grade>>("api/grades");
        }
    }
}
