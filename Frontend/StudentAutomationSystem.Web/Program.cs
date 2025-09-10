using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StudentAutomationSystem.Web;
using StudentAutomationSystem.Web.Services;
using StudentAutomationSystem.Web.Services.Handlers;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient yapılandırması - Backend URL'ini kendi backend adresinizle değiştirin
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });

// Local Storage
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthMessageHandler>();

// Custom Services
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
// Authorization
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
