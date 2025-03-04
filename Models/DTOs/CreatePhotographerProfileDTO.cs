using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models.DTOs
{
    public class CreatePhotographerProfileDTO
    {
        [Required]
        public string UserId { get; set; }
        
        public string? Bio { get; set; }
        
        [DataType(DataType.Url)]
        public string? PortfolioLink { get; set; }
        
        public string? ProfileImage { get; set; }
        
        public string? ContactInfo { get; set; }
        
        public bool IsVerified { get; set; } = false;
    }
} 