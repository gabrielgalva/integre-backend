using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using IntegreBackend.Models;
using IntegreBackend.Dtos;
using IntegreBackend.Data;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CompanyAuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public CompanyAuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCompanyDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.ResponsibleName,
            Role = "Company"
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var company = new Company
        {
            CompanyName = dto.CompanyName,
            Cnpj = dto.Cnpj,
            Address = dto.Address,
            ResponsibleName = dto.ResponsibleName,
            ResponsibleRole = dto.ResponsibleRole,
            UserId = user.Id
        };

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        return Ok("Empresa registrada com sucesso!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCompanyDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
        if (!result.Succeeded)
            return Unauthorized("Credenciais inválidas.");

        return Ok("Login realizado com sucesso!");
    }
}
