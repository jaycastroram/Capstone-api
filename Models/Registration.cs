using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class Registration
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? ImageLocation { get; set; }

        [Required]
        public string Role { get; set; }

        public bool IsVerified { get; set; } = false;
    }
} 