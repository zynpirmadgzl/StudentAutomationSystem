using Microsoft.EntityFrameworkCore;
using StudentAutomationSystem.API.Data;
using StudentAutomationSystem.API.DTOs;
using StudentAutomationSystem.API.Models;

namespace StudentAutomationSystem.API.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext _context;

        public TeacherService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
        {
            return await _context.Teachers
                .Include(t => t.User)
                .Select(t => new TeacherDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName,
                    Email = t.User.Email!,
                    EmployeeNumber = t.EmployeeNumber,
                    Department = t.Department,
                    IsActive = t.User.IsActive
                })
                .ToListAsync();
        }

        public async Task<TeacherDto?> GetTeacherByIdAsync(int id)
        {
            var teacher = await _context.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return null;

            return new TeacherDto
            {
                Id = teacher.Id,
                UserId = teacher.UserId,
                FirstName = teacher.User.FirstName,
                LastName = teacher.User.LastName,
                Email = teacher.User.Email!,
                EmployeeNumber = teacher.EmployeeNumber,
                Department = teacher.Department,
                IsActive = teacher.User.IsActive
            };
        }

        public async Task<TeacherDto?> CreateTeacherAsync(CreateTeacherDto createTeacherDto)
        {
            var user = new User
            {
                UserName = createTeacherDto.Email,
                Email = createTeacherDto.Email,
                FirstName = createTeacherDto.FirstName,
                LastName = createTeacherDto.LastName,
                Role = UserRole.Teacher
            };

            var teacher = new Teacher
            {
                User = user,
                EmployeeNumber = createTeacherDto.EmployeeNumber,
                Department = createTeacherDto.Department
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return new TeacherDto
            {
                Id = teacher.Id,
                UserId = teacher.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                EmployeeNumber = teacher.EmployeeNumber,
                Department = teacher.Department,
                IsActive = user.IsActive
            };
        }

        public async Task<TeacherDto?> UpdateTeacherAsync(int id, UpdateTeacherDto updateTeacherDto)
        {
            var teacher = await _context.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return null;

            teacher.User.FirstName = updateTeacherDto.FirstName;
            teacher.User.LastName = updateTeacherDto.LastName;
            teacher.User.Email = updateTeacherDto.Email;
            teacher.User.UserName = updateTeacherDto.Email;
            teacher.Department = updateTeacherDto.Department;

            await _context.SaveChangesAsync();

            return new TeacherDto
            {
                Id = teacher.Id,
                UserId = teacher.UserId,
                FirstName = teacher.User.FirstName,
                LastName = teacher.User.LastName,
                Email = teacher.User.Email!,
                EmployeeNumber = teacher.EmployeeNumber,
                Department = teacher.Department,
                IsActive = teacher.User.IsActive
            };
        }

        public async Task<bool> DeleteTeacherAsync(int id)
        {
            var teacher = await _context.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return false;

            teacher.User.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
