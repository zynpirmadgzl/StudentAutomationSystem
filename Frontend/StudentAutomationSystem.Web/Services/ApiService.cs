using Blazored.LocalStorage;
using StudentAutomationSystem.Web.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;


namespace StudentAutomationSystem.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly ILogger<ApiService> _logger;
        
        public ApiService(HttpClient httpClient, ILocalStorageService localStorage, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _logger = logger;
        }
        
        public async Task SetAuthTokenAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
        
        #region Auth Methods
        
        public async Task<AuthResponse?> LoginAsync(LoginModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    if (result != null)
                    {
                        await _localStorage.SetItemAsync("authToken", result.Token);
                        await _localStorage.SetItemAsync("currentUser", result);
                        await SetAuthTokenAsync();
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error");
            }
            return null;
        }
        
        public async Task<AuthResponse?> RegisterAsync(RegisterModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    if (result != null)
                    {
                        await _localStorage.SetItemAsync("authToken", result.Token);
                        await _localStorage.SetItemAsync("currentUser", result);
                        await SetAuthTokenAsync();
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register error");
            }
            return null;
        }
        
        public async Task<bool> LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("currentUser");
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return true;
        }
        
        public async Task<AuthResponse?> GetCurrentUserAsync()
        {
            return await _localStorage.GetItemAsync<AuthResponse>("currentUser");
        }
        
        #endregion
        
        #region Student Methods
        
        public async Task<List<StudentModel>?> GetStudentsAsync()
        {
            try
            {
                await SetAuthTokenAsync();
                return await _httpClient.GetFromJsonAsync<List<StudentModel>>("api/students");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting students");
                return null;
            }
        }
        
        public async Task<StudentModel?> GetStudentByIdAsync(int id)
        {
            try
            {
                await SetAuthTokenAsync();
                return await _httpClient.GetFromJsonAsync<StudentModel>($"api/students/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student by id");
                return null;
            }
        }
        
        public async Task<StudentModel?> CreateStudentAsync(CreateStudent model)
        {
            try
            {
                await SetAuthTokenAsync();
                var response = await _httpClient.PostAsJsonAsync("api/students", model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<StudentModel>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student");
            }
            return null;
        }
        
        public async Task<StudentModel?> UpdateStudentAsync(int id, UpdateStudent model)
        {
            try
            {
                await SetAuthTokenAsync();
                var response = await _httpClient.PutAsJsonAsync($"api/students/{id}", model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<StudentModel>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student");
            }
            return null;
        }
        
        public async Task<bool> DeleteStudentAsync(int id)
        {
            try
            {
                await SetAuthTokenAsync();
                var response = await _httpClient.DeleteAsync($"api/students/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student");
                return false;
            }
        }
        
        #endregion
        
        #region Teacher Methods
        
        public async Task<List<TeacherModel>?> GetTeachersAsync()
        {
            try
            {
                await SetAuthTokenAsync();
                return await _httpClient.GetFromJsonAsync<List<TeacherModel>>("api/teachers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting teachers");
                return null;
            }
        }
        
        public async Task<TeacherModel?> CreateTeacherAsync(CreateTeacher model)
        {
            try
            {
                await SetAuthTokenAsync();
                var response = await _httpClient.PostAsJsonAsync("api/teachers", model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TeacherModel>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating teacher");
            }
            return null;
        }
        
        #endregion
        
        #region Course Methods
        
        public async Task<List<CourseModel>?> GetCoursesAsync()
        {
            try
            {
                await SetAuthTokenAsync();
                return await _httpClient.GetFromJsonAsync<List<CourseModel>>("api/courses");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting courses");
                return null;
            }
        }
        
        public async Task<CourseModel?> GetCourseByIdAsync(int id)
        {
            try
            {
                await SetAuthTokenAsync();
                return await _httpClient.GetFromJsonAsync<CourseModel>($"api/courses/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting course by id");
                return null;
            }
        }
        
        public async Task<CourseModel?> CreateCourseAsync(CreateCourse model)
        {
            try
            {
                await SetAuthTokenAsync();
                var response = await _httpClient.PostAsJsonAsync("api/courses", model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CourseModel>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course");
            }
            return null;
        }
        
        public async Task<bool> AddStudentToCourseAsync(int courseId, int studentId)
        {
            try
            {
                await SetAuthTokenAsync();
                var response = await _httpClient.PostAsync($"api/courses/{courseId}/students/{studentId}", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding student to course");
                return false;
            }
        }
        
        public async Task<bool> RemoveStudentFromCourseAsync(int courseId, int studentId)
        {
            try
            {
                await SetAuthTokenAsync();
                var response = await _httpClient.DeleteAsync($"api/courses/{courseId}/students/{studentId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing student from course");
                return false;
            }
        }
        
        #endregion
        
        #region Grade Methods
        
        public async Task<List<GradeModel>?> GetStudentGradesAsync(int studentId)
        {
            try
            {
                await SetAuthTokenAsync();
                return await _httpClient.GetFromJsonAsync<List<GradeModel>>($"api/students/{studentId}/grades");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student grades");
                return null;
            }
        }
        
        public async Task<GradeModel?> CreateGradeAsync(CreateGrade model)
        {
            try
            {
                await SetAuthTokenAsync();
                var response = await _httpClient.PostAsJsonAsync("api/grades", model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<GradeModel>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating grade");
            }
            return null;
        }
        
        #endregion
    }
}