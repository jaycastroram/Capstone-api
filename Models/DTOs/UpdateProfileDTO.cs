using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models.DTOs
{
    public class UpdateProfileDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? ImageLocation { get; set; }
    }
} 