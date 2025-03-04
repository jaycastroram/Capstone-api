using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models.DTOs
{
    public class SetPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
} 