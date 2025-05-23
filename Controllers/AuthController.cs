using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IntegreBackend.Models;
using IntegreBackend.Dtos;
using IntegreBackend.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace IntegreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.Select(u => new
            {
                u.Id,
                u.Email,
                u.Nome,
                u.Cargo
            }).ToList();

            return Ok(users);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("Usuário não encontrado.");

            return Ok(new
            {
                user.Id,
                user.Email,
                user.Nome,
                user.Cargo
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Senha))
                return BadRequest("Email e senha são obrigatórios.");

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                Nome = dto.Nome,
                Cargo = dto.Cargo
            };

            var result = await _userManager.CreateAsync(user, dto.Senha);

            if (result.Succeeded)
                return Ok(new { message = "Usuário registrado com sucesso!" });

            return BadRequest(result.Errors);
        }

        [HttpPost("register-backend")]
        public async Task<IActionResult> RegisterBackend([FromBody] RegisterBackend dto)
        {
            return await RegisterUser(dto.Email, dto.Password, dto.NomeCompleto, "backend");
        }

        [HttpPost("register-frontend")]
        public async Task<IActionResult> RegisterFrontend([FromBody] RegisterFrontend dto)
        {
            return await RegisterUser(dto.Email, dto.Password, dto.NomeCompleto, "frontend");
        }

        [HttpPost("register-database")]
        public async Task<IActionResult> RegisterDatabase([FromBody] RegisterDatabase dto)
        {
            return await RegisterUser(dto.Email, dto.Password, dto.NomeCompleto, "database");
        }

        [HttpPost("register-company")]
        public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyDto dto)
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

            return Ok(new { message = "Empresa registrada com sucesso!" });
        }

        private async Task<IActionResult> RegisterUser(string email, string password, string nome, string cargo)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return BadRequest("Email e senha são obrigatórios.");

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Nome = nome,
                Cargo = cargo
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
                return Ok(new { message = "Usuário registrado com sucesso!" });

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Senha))
                return BadRequest("Email e senha são obrigatórios.");

            // Tenta autenticar como usuário
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null)
            {
                var isValidPassword = await _userManager.CheckPasswordAsync(user, dto.Senha);
                if (!isValidPassword) return Unauthorized("Senha incorreta.");

                var token = GenerateJwtToken(user.Email ?? "", user.Cargo ?? "", user.Id);

                return Ok(new
                {
                    token,
                    redirectTo = user.Cargo?.ToLower() switch
                    {
                        "administrador" => "/Admin",
                        "aluno" => "/Aluno",
                        "rh" => "/Company",
                        "chefe" => "/Chefe",
                        "backend" => "/Backend",
                        "frontend" => "/Frontend",
                        "database" => "/Database",
                        _ => "/"
                    }
                });
            }

            // Tenta autenticar como empresa
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Email == dto.Email);
            if (company == null)
                return Unauthorized("Usuário ou empresa não encontrado.");

            var senhaHash = ComputeSha256Hash(dto.Senha);
            if (company.SenhaHash != senhaHash)
                return Unauthorized("Senha incorreta.");

            var tokenCompany = GenerateJwtToken(company.Email ?? "", company.CargoResponsavel ?? "empresa", company.Id.ToString());

            return Ok(new
            {
                token = tokenCompany,
                redirectTo = company.CargoResponsavel?.ToLower() == "chefe" ? "/Chefe" : "/Company"
            });
        }

        private string GenerateJwtToken(string email, string cargo, string id)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim("Cargo", cargo),
                new Claim("UserId", id),
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

        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToBase64String(bytes);
        }
    }
}
