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
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? ImageLocation { get; set; }

        public string Role { get; set; } = "user";
    }
} 