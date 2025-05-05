namespace IntegreBackend.Models
{
    public class Company
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty; // Nome da empresa

        public string Cnpj { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string ResponsibleName { get; set; } = string.Empty;

        public string ResponsibleRole { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty; // Recomendado usar Identity para isso
    }
}
