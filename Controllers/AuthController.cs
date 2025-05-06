using IntegreBackend.Models;
using IntegreBackend.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;  // Adicionando para acessar a configuração
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IntegreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;  // Adicionando para acessar a configuração

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;  // Injetando a configuração
        }

        // Registro da empresa
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                // Verifica se os campos obrigatórios estão presentes
                if (string.IsNullOrWhiteSpace(model.Email) || 
                    string.IsNullOrWhiteSpace(model.Password) ||
                    string.IsNullOrWhiteSpace(model.Name))
                {
                    return BadRequest(new { message = "Email, senha e nome da empresa são obrigatórios." });
                }

                // Verifica se o usuário já existe
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Este email já está em uso." });
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Cnpj = model.Cnpj,
                    Address = model.Address,
                    CompanyName = model.Name,
                    ResponsibleName = model.ResponsibleName,
                    ResponsibleRole = model.ResponsibleRole
                };

                // Criação do usuário
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Empresa registrada com sucesso!" });
                }

                // Logando e retornando erros
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { errors });
            }
            catch (Exception ex)
            {
                // Logando exceção
                Console.WriteLine($"Erro ao registrar empresa: {ex.Message}");
                return StatusCode(500, new { message = "Erro interno do servidor." });
            }
        }

        // Login do usuário (empresa)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }

            return Unauthorized("Credenciais inválidas");
        }

        // Geração do JWT token
       // Geração do JWT token
private string GenerateJwtToken(ApplicationUser user)
{
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        // Você pode adicionar mais claims se necessário (como 'Cnpj', 'CompanyName', etc)
    };

    // Lê a chave secreta do arquivo de configuração
    var secretKey = _configuration["Jwt:Key"];
    
    // Verifica se a chave secreta está nula ou vazia
    if (string.IsNullOrEmpty(secretKey))
    {
        throw new InvalidOperationException("A chave secreta para gerar o JWT não foi configurada corretamente.");
    }

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],  // Lê o Issuer do arquivo de configuração
        audience: _configuration["Jwt:Audience"],  // Lê o Audience do arquivo de configuração
        claims: claims,
        expires: DateTime.UtcNow.AddHours(2),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

    }
}
