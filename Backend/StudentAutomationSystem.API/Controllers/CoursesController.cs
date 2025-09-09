using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentAutomationSystem.API.DTOs;
using StudentAutomationSystem.API.Services;

namespace StudentAutomationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
                return NotFound();
                
            return Ok(course);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
        {
            var course = await _courseService.CreateCourseAsync(createCourseDto);
            if (course == null)
                return BadRequest("Course code already exists or creation failed");
                
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }
        
        [HttpPost("{courseId}/students/{studentId}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> AddStudentToCourse(int courseId, int studentId)
        {
            var result = await _courseService.AddStudentToCourseAsync(courseId, studentId);
            if (!result)
                return BadRequest("Failed to add student to course");
                
            return Ok();
        }
        
        [HttpDelete("{courseId}/students/{studentId}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> RemoveStudentFromCourse(int courseId, int studentId)
        {
            var result = await _courseService.RemoveStudentFromCourseAsync(courseId, studentId);
            if (!result)
                return BadRequest("Failed to remove student from course");
                
            return Ok();
        }
    }
}