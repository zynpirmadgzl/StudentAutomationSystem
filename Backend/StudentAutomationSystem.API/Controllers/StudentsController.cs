using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentAutomationSystem.API.DTOs;
using StudentAutomationSystem.API.Services;

namespace StudentAutomationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(string id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound();
            
            return Ok(student);
        }

        [HttpGet("me")]
        [Authorize] 
        public async Task<ActionResult<StudentDto>> GetMe()
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    Console.WriteLine("Token içinde NameIdentifier claim bulunamadı.");
                    Console.WriteLine($"Available claims: {string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}"))}");
                    return Unauthorized("Token geçerli değil veya claim eksik.");
                }

                var userId = userIdClaim.Value;
                var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                
                Console.WriteLine($"GetMe çağrıldı. UserId: {userId}, Role: {userRole}");

                if (userRole != "Student")
                {
                    Console.WriteLine($"Bu endpoint sadece Student rolü için. Mevcut rol: {userRole}");
                    return Forbid("Bu endpoint sadece öğrenciler için.");
                }

                var student = await _studentService.GetStudentByIdAsync(userId);

                if (student == null)
                {
                    Console.WriteLine("Öğrenci bulunamadı.");
                    return NotFound("Öğrenci bulunamadı. Lütfen sistem yöneticisi ile iletişime geçin.");
                }

                Console.WriteLine($"Öğrenci bulundu: {student.FirstName} {student.LastName}");
                return Ok(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetMe hatası: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<StudentDto>> CreateStudent([FromBody] CreateStudentDto createStudentDto)
        {
            var student = await _studentService.CreateStudentAsync(createStudentDto);
            if (student == null)
                return BadRequest("Student number already exists or creation failed");
            
            return CreatedAtAction(nameof(GetStudent), new { id = student.UserId }, student);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<StudentDto>> UpdateStudent(int id, [FromBody] UpdateStudentDto updateStudentDto)
        {
            var student = await _studentService.UpdateStudentAsync(id, updateStudentDto);
            if (student == null)
                return NotFound();
            
            return Ok(student);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            if (!result)
                return NotFound();
            
            return NoContent();
        }

        [HttpGet("{id}/grades")]
        public async Task<ActionResult<IEnumerable<GradeDto>>> GetStudentGrades(int id)
        {
            var grades = await _studentService.GetStudentGradesAsync(id);
            return Ok(grades);
        }
    }
}