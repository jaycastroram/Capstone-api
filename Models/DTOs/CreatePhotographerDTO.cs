namespace Capstone.Api.Models.DTOs
{
    public class CreatePhotographerDTO
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Bio { get; set; }
        public string? PortfolioLink { get; set; }
    }
} 