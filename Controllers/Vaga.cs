using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using IntegreBackend.Models;
using IntegreBackend.Dtos;
using IntegreBackend.Data;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;


[ApiController]
[Route("api/[controller]")]
public class VagaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public VagaController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Criar Vaga
    [HttpPost("create")]
    public async Task<IActionResult> CreateVaga([FromBody] VagaDto dto)
    {
        var vaga = new Vaga
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Area = dto.Area
        };

        _context.Vagas.Add(vaga);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Vaga criada com sucesso!" });
    }

    // Listar Vagas
    [HttpGet("list")]
    public async Task<IActionResult> ListVagas()
    {
        var vagas = await _context.Vagas.ToListAsync();
        return Ok(vagas);
    }

    // Editar Vaga
    [HttpPut("edit/{id}")]
    public async Task<IActionResult> EditVaga(int id, [FromBody] VagaDto dto)
    {
        var vaga = await _context.Vagas.FindAsync(id);
        if (vaga == null)
            return NotFound(new { message = "Vaga não encontrada." });

        vaga.Nome = dto.Nome;
        vaga.Descricao = dto.Descricao;
        vaga.Area = dto.Area;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Vaga atualizada com sucesso!" });
    }

    // Excluir Vaga
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteVaga(int id)
    {
        var vaga = await _context.Vagas.FindAsync(id);
        if (vaga == null)
            return NotFound(new { message = "Vaga não encontrada." });

        _context.Vagas.Remove(vaga);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Vaga removida com sucesso!" });
    }
}
