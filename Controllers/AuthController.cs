using IntegreBackend.Models;
using IntegreBackend.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace IntegreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("register/frontend")]
        public async Task<IActionResult> RegisterFrontend([FromBody] RegisterFrontendDto model)
        {
            return await RegisterUser(model, "FrontendDev");
        }

        [HttpPost("register/backend")]
        public async Task<IActionResult> RegisterBackend([FromBody] RegisterBackendDto model)
        {
            return await RegisterUser(model, "BackendDev");
        }

        [HttpPost("register/database")]
        public async Task<IActionResult> RegisterDatabase([FromBody] RegisterDatabaseDto model)
        {
            return await RegisterUser(model, "DatabaseDev");
        }

        private async Task<IActionResult> RegisterUser(dynamic model, string role)
        {
            if (string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password) ||
                string.IsNullOrWhiteSpace(model.FullName))
            {
                return BadRequest(new { message = "Email, senha e nome são obrigatórios." });
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Este email já está em uso." });
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                Linkedin = model.Linkedin,
                Github = model.Github,
                Matricula = model.Matricula,
                Role = role
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { message = $"Usuário {role} registrado com sucesso!" });
            }

            var errors = ((IEnumerable<IdentityError>)result.Errors).Select(e => e.Description).ToList();
            return BadRequest(new { errors });
        }

        [HttpPost("login/frontend")]
        public async Task<IActionResult> LoginFrontend([FromBody] LoginFrontDto model)
        {
            return await Login(model.Email, model.Password, "FrontendDev");
        }

        [HttpPost("login/backend")]
        public async Task<IActionResult> LoginBackend([FromBody] LoginBackDto model)
        {
            return await Login(model.Email, model.Password, "BackendDev");
        }

        [HttpPost("login/database")]
        public async Task<IActionResult> LoginDatabase([FromBody] LoginBancoDto model)
        {
            return await Login(model.Email, model.Password, "DatabaseDev");
        }

        private async Task<IActionResult> Login(string email, string password, string expectedRole)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                if (user.Role == expectedRole)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new { token });
                }
                return Unauthorized("Usuário não pertence à categoria esperada.");
            }

            return Unauthorized("Credenciais inválidas");
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role ?? "")
            };

            var secretKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("A chave JWT não está configurada.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
