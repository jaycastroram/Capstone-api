using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class ClientFollowup
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public int PhotographerId { get; set; }

        [Required]
        public string Method { get; set; } // email, text, phone

        public string? FollowupNotes { get; set; }

        public DateTime FollowupDate { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = "pending"; // pending, in progress, resolved

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Photographer Photographer { get; set; }
    }
} 