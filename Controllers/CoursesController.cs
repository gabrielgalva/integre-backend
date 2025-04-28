using IntegreBackend.Data;
using IntegreBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntegreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _context.Courses.ToListAsync();
            return Ok(courses);
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return Ok(course);
        }
    }
}
