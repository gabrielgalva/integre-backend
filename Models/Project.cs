namespace IntegreBackend.Models
{
    public class Project
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
