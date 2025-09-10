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

         public async Task<List<GradeModel>> GetAllGradesAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<GradeModel>>("api/grades");
            return result ?? new List<GradeModel>();
        }

        public async Task<List<GradeModel>> GetMyGradesAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<GradeModel>>("api/grades/my");
            return result ?? new List<GradeModel>();
        }
    }
}
