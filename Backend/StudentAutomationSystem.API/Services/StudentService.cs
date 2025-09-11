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

        public async Task<StudentDto?> GetStudentByIdAsync(string userId)
        {
            Console.WriteLine($"GetStudentByIdAsync çağrıldı. UserId: {userId}");
            
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.User.IsActive);

            if (student == null)
            {
                Console.WriteLine($"Öğrenci bulunamadı. UserId: {userId}");
                
                // Debug: Kullanıcının var olup olmadığını kontrol et
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    Console.WriteLine($"Kullanıcı hiç bulunamadı: {userId}");
                }
                else
                {
                    Console.WriteLine($"Kullanıcı bulundu ama Student kaydı yok. User: {user.FirstName} {user.LastName}, Role: {user.Role}");
                    
                    // Eğer kullanıcı Student rolündeyse ama Student kaydı yoksa, oluştur
                    if (user.Role == UserRole.Student)
                    {
                        var newStudent = new Student
                        {
                            UserId = user.Id,
                            StudentNumber = $"S{DateTime.Now.Ticks}", // Geçici öğrenci numarası
                            EnrollmentDate = DateTime.UtcNow
                        };
                        
                        _context.Students.Add(newStudent);
                        await _context.SaveChangesAsync();
                        
                        Console.WriteLine($"Eksik Student kaydı oluşturuldu: {newStudent.StudentNumber}");
                        
                        return new StudentDto
                        {
                            Id = newStudent.Id,
                            UserId = newStudent.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email!,
                            StudentNumber = newStudent.StudentNumber,
                            EnrollmentDate = newStudent.EnrollmentDate,
                            IsActive = user.IsActive
                        };
                    }
                }
                
                return null;
            }

            Console.WriteLine($"Öğrenci bulundu: {student.User.FirstName} {student.User.LastName}, StudentNumber: {student.StudentNumber}");

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
            if (await _context.Students.AnyAsync(s => s.StudentNumber == createStudentDto.StudentNumber))
                return null;

            var user = new User
            {
                UserName = createStudentDto.Email,
                Email = createStudentDto.Email,
                FirstName = createStudentDto.FirstName,
                LastName = createStudentDto.LastName,
                Role = UserRole.Student,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, createStudentDto.Password);
            if (!result.Succeeded) return null;

            var student = new Student
            {
                UserId = user.Id,
                StudentNumber = createStudentDto.StudentNumber,
                EnrollmentDate = DateTime.UtcNow
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