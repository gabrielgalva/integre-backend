namespace IntegreBackend.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? NomeEmpresa { get; set; }
        public string? NomeResponsavel { get; set; }
        public string? CargoResponsavel { get; set; }
        public string? CNPJ { get; set; }
        public string? Endereco { get; set; }
        public string? SenhaHash { get; set; }
    }
}
