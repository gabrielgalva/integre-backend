using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntegreBackend.Data;
using IntegreBackend.Models;
using IntegreBackend.Dtos;

namespace IntegreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidaturaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CandidaturaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCandidatura([FromBody] CandidaturaDto dto)
        {
            var candidaturaExistente = await _context.Candidaturas
                .FirstOrDefaultAsync(c => c.IdVaga == dto.IdVaga && c.Linkedin == dto.Linkedin);

            if (candidaturaExistente != null)
            {
                return BadRequest(new { message = "Você já se candidatou a essa vaga!" });
            }

            var candidatura = new Candidatura
            {
                IdVaga = dto.IdVaga,
                Linkedin = dto.Linkedin
            };

            _context.Candidaturas.Add(candidatura);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Candidatura realizada com sucesso!" });
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListCandidaturas()
        {
            var candidaturas = await _context.Candidaturas.ToListAsync();
            return Ok(candidaturas);
        }
    }
}
