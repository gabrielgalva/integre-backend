using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntegreBackend.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string Cnpj { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string ResponsibleName { get; set; } = string.Empty;

        public string ResponsibleRole { get; set; } = string.Empty;

        // Relacionamento com ApplicationUser
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }
    }
}
