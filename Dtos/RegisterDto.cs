namespace IntegreBackend.Dtos
{
    public class RegisterDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "Student" ou "Company"
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
