namespace IntegreBackend.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Linkedin { get; set; } = string.Empty;
        public string Github { get; set; } = string.Empty;
        public string Matricula { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
