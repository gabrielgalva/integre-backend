using IntegreBackend.Data;
using IntegreBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntegreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobVacanciesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobVacanciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/JobVacancies
        [HttpGet]
        public async Task<IActionResult> GetAllJobVacancies()
        {
            var vacancies = await _context.JobVacancies
                .Include(j => j.Company) // Inclui o usuário da empresa
                .ToListAsync();

            return Ok(vacancies);
        }

        // POST: api/JobVacancies
        [HttpPost]
        public async Task<IActionResult> CreateJobVacancy([FromBody] JobVacancy jobVacancy)
        {
            _context.JobVacancies.Add(jobVacancy);
            await _context.SaveChangesAsync();
            return Ok(jobVacancy);
        }
    }
}
