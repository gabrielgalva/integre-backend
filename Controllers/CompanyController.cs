using IntegreBackend.Data;
using IntegreBackend.Dtos;
using IntegreBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace IntegreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCompanyDto dto)
        {
            if (await _context.Companies.AnyAsync(c => c.Email == dto.Email))
                return BadRequest("Empresa já cadastrada com este e-mail.");

            var company = new Company
            {
                Email = dto.Email,
                NomeEmpresa = dto.NomeEmpresa,
                NomeResponsavel = dto.NomeResponsavel,
                CargoResponsavel = dto.CargoResponsavel,
                CNPJ = dto.CNPJ,
                Endereco = dto.Endereco,
                SenhaHash = ComputeSha256Hash(dto.Senha ?? "")
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return Ok("Empresa cadastrada com sucesso!");
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToBase64String(bytes);
        }
    }
}
