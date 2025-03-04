namespace Capstone.Api.Models.DTOs
{
    public class UpdatePhotographerDTO
    {
        public required string Name { get; set; }
        public string? Bio { get; set; }
        public string? PortfolioLink { get; set; }
        public string? Password { get; set; }  // Optional for updates
    }
} 