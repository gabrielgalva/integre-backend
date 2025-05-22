using IntegreBackend.Data;
using IntegreBackend.Dtos;
using IntegreBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IntegreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public CompanyController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

            return Ok(new { message = "Empresa cadastrada com sucesso!" });
        }

      [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto dto)
{
    if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Senha))
        return BadRequest("Email e senha são obrigatórios.");

    var company = await _context.Companies.FirstOrDefaultAsync(c => c.Email == dto.Email);
    if (company == null)
        return Unauthorized("Empresa não encontrada.");

    var senhaHash = ComputeSha256Hash(dto.Senha);
    if (company.SenhaHash != senhaHash)
        return Unauthorized("Senha incorreta.");

    var token = GenerateJwtToken(company);

    var cargo = company.CargoResponsavel?.ToLower();

    return Ok(new
    {
        token,
        redirectTo = cargo == "chefe" ? "/Chefe" : "/Company"
    });
}

        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToBase64String(bytes);
        }

        private string GenerateJwtToken(Company company)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, company.Email ?? ""),
                new Claim("Cargo", "empresa"),
                new Claim("CompanyId", company.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing in configuration.")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
