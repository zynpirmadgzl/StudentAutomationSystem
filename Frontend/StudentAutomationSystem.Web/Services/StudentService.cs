using System.Net.Http.Json;
using StudentAutomationSystem.Web.Models;

namespace StudentAutomationSystem.Web.Services
{
    

    public class StudentService : IStudentService
    {
        private readonly HttpClient _httpClient;

        public StudentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
public async Task<List<StudentModel>> GetAllStudentsAsync()
{
    var result = await _httpClient.GetFromJsonAsync<List<StudentModel>>("api/students");
    return result ?? new List<StudentModel>();
}

         public async Task<StudentModel> GetMyProfileAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<StudentModel>("api/students/me");
            return result ?? new StudentModel();
        }
    }
    
}
