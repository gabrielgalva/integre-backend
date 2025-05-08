using Microsoft.AspNetCore.Identity;

namespace IntegreBackend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string Linkedin { get; set; } = string.Empty;
        public string Github { get; set; } = string.Empty;
        public string Matricula { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
