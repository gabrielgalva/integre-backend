using Microsoft.AspNetCore.Identity;

namespace IntegreBackend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string FullName { get; set; }
        public required string Role { get; set; }  // Student ou Company
    }
}
