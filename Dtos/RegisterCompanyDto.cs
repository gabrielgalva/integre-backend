namespace IntegreBackend.Dtos
{
    public class RegisterCompanyDto
    {
        public string? Email { get; set; }
        public string? NomeEmpresa { get; set; }
        public string? NomeResponsavel { get; set; }
        public string? CargoResponsavel { get; set; }
        public string? CNPJ { get; set; }
        public string? Endereco { get; set; }
        public string? Senha { get; set; }
    }
}
