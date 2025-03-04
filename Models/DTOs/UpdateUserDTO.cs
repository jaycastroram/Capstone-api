namespace Capstone.Api.Models.DTOs
{
    public class UpdateUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsVerified { get; set; }
        public string? Bio { get; set; }
        public string? PortfolioLink { get; set; }
    }
} 