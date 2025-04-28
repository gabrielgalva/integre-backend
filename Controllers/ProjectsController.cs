using IntegreBackend.Data;
using IntegreBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntegreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _context.Projects.ToListAsync();
            return Ok(projects);
        }

        // POST: api/Projects
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return Ok(project);
        }
    }
}
