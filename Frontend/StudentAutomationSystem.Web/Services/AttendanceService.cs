using System.Net.Http.Json;
using StudentAutomationSystem.Web.Models;

namespace StudentAutomationSystem.Web.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly HttpClient _httpClient;

        public AttendanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AttendanceModel>> GetMyAttendanceAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<AttendanceModel>>("api/attendances");
            return result ?? new List<AttendanceModel>();
        }
    }
}
