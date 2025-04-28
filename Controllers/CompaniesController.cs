using IntegreBackend.Models;
using IntegreBackend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntegreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Protege todas as rotas com JWT
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        // GET: api/Companies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(Guid id)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // POST: api/Companies
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            company.Id = Guid.NewGuid(); // Gera o GUID aqui

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, company);
        }
    }
}
