using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using StudentAutomationSystem.Web.Models;

namespace StudentAutomationSystem.Web.Services
{
    public class StudentService : IStudentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public StudentService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<StudentModel?> GetMyProfileAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                var student = await _httpClient.GetFromJsonAsync<StudentModel>("api/students/me");
                return student;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return null;
            }
        }
        public async Task<List<StudentModel>> GetAllStudentsAsync()
        {
            var students = await _httpClient.GetFromJsonAsync<List<StudentModel>>("api/students");
            return students ?? new List<StudentModel>();
        }
    }
}
