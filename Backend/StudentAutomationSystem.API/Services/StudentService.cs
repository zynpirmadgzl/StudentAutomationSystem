
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentAutomationSystem.API.Data;
using StudentAutomationSystem.API.DTOs;
using StudentAutomationSystem.API.Models;

namespace StudentAutomationSystem.API.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        
        public StudentService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.User)
                .Where(s => s.User.IsActive)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    Email = s.User.Email!,
                    StudentNumber = s.StudentNumber,
                    EnrollmentDate = s.EnrollmentDate,
                    IsActive = s.User.IsActive
                })
                .ToListAsync();
        }
        
        public async Task<StudentDto?> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
                
            if (student == null) return null;
            
            return new StudentDto
            {
                Id = student.Id,
                UserId = student.UserId,
                FirstName = student.User.FirstName,
                LastName = student.User.LastName,
                Email = student.User.Email!,
                StudentNumber = student.StudentNumber,
                EnrollmentDate = student.EnrollmentDate,
                IsActive = student.User.IsActive
            };
        }
        
        public async Task<StudentDto?> CreateStudentAsync(CreateStudentDto createStudentDto)
        {
            // Check if student number already exists
            if (await _context.Students.AnyAsync(s => s.StudentNumber == createStudentDto.StudentNumber))
                return null;
                
            var user = new User
            {
                UserName = createStudentDto.Email,
                Email = createStudentDto.Email,
                FirstName = createStudentDto.FirstName,
                LastName = createStudentDto.LastName,
                Role = UserRole.Student
            };
            
            var result = await _userManager.CreateAsync(user, createStudentDto.Password);
            if (!result.Succeeded) return null;
            
            var student = new Student
            {
                UserId = user.Id,
                StudentNumber = createStudentDto.StudentNumber
            };
            
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            
            return new StudentDto
            {
                Id = student.Id,
                UserId = student.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                StudentNumber = student.StudentNumber,
                EnrollmentDate = student.EnrollmentDate,
                IsActive = user.IsActive
            };
        }
        
        public async Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentDto updateStudentDto)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
                
            if (student == null) return null;
            
            student.User.FirstName = updateStudentDto.FirstName;
            student.User.LastName = updateStudentDto.LastName;
            student.User.Email = updateStudentDto.Email;
            student.User.UserName = updateStudentDto.Email;
            
            await _context.SaveChangesAsync();
            
            return new StudentDto
            {
                Id = student.Id,
                UserId = student.UserId,
                FirstName = student.User.FirstName,
                LastName = student.User.LastName,
                Email = student.User.Email,
                StudentNumber = student.StudentNumber,
                EnrollmentDate = student.EnrollmentDate,
                IsActive = student.User.IsActive
            };
        }
        
        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
                
            if (student == null) return false;
            
            student.User.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<IEnumerable<GradeDto>> GetStudentGradesAsync(int studentId)
        {
            return await _context.Grades
                .Include(g => g.Course)
                .Where(g => g.StudentId == studentId)
                .Select(g => new GradeDto
                {
                    Id = g.Id,
                    StudentId = g.StudentId,
                    CourseId = g.CourseId,
                    CourseName = g.Course.CourseName,
                    Score = g.Score,
                    GradeType = g.GradeType,
                    Comments = g.Comments,
                    CreatedAt = g.CreatedAt
                })
                .ToListAsync();
        }
    }
}