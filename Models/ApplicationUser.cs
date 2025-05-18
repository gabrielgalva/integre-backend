using Microsoft.AspNetCore.Identity;

namespace IntegreBackend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Nome { get; set; }
        public string? Cargo { get; set; } // Aluno, EmpresaRH, EmpresaChefe, Administrador
    }
}
