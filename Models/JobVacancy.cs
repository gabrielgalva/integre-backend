namespace IntegreBackend.Models
{
    public class JobVacancy
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string CompanyId { get; set; }  // FK
        public required ApplicationUser Company { get; set; }  // Navigation property
    }
}
