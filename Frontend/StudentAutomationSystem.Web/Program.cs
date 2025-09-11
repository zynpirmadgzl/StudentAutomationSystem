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

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5001/") });

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthMessageHandler>();

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
builder.Services.AddBlazoredLocalStorage();

// Authorization
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
