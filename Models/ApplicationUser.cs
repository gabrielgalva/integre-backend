using Microsoft.AspNetCore.Identity;

namespace IntegreBackend.Models
{
    public class ApplicationUser : IdentityUser
    {public string Cnpj { get; set; } = string.Empty;        // CNPJ da empresa
    public string Address { get; set; } = string.Empty;     // Endereço da empresa
    public string CompanyName { get; set; } = string.Empty; // Nome da empresa
    public string? ResponsibleName { get; set; }
    public string ResponsibleRole { get; set; } = string.Empty; // Cargo do responsável
    }
}

