namespace Capstone.Api.Models.DTOs
{
    public class PhotographerDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string? Bio { get; set; }
        public string? PortfolioLink { get; set; }
        public UserDTO? User { get; set; }
        public ICollection<PackageDTO>? Packages { get; set; }
    }
} 