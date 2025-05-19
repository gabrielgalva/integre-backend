using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IntegreBackend.Models;
using IntegreBackend.Dtos;

namespace IntegreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
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

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return Unauthorized("Usuário não encontrado.");

            var isValidPassword = await _userManager.CheckPasswordAsync(user, dto.Senha);
            if (!isValidPassword) return Unauthorized("Senha incorreta.");

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                redirectTo = user.Cargo?.ToLower() switch
                {
                    "administrador" => "/admin",
                    "aluno" => "/aluno",
                    "empresarh" => "/empresa/rh",
                    "empresachefe" => "/empresa/chefe",
                    "backend" => "/backend",
                    "frontend" => "/frontend",
                    "database" => "/database",
                    _ => "/"
                }
            });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? ""),
                new Claim("Cargo", user.Cargo ?? ""),
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
