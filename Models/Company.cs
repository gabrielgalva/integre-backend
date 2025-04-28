namespace IntegreBackend.Models
{
    public class Company
    {
        public Guid Id { get; set; } // Guid sem inicializar aqui
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
