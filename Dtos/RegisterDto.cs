using System.ComponentModel.DataAnnotations;

namespace IntegreBackend.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        public string Name { get; set; } = string.Empty;  // Company name
        
        public string Cnpj { get; set; } = string.Empty;
        
        public string Address { get; set; } = string.Empty;
        
        public string ResponsibleName { get; set; } = string.Empty;
        
        public string ResponsibleRole { get; set; } = string.Empty;
    }
}