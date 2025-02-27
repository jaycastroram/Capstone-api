using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class Inquiry
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public int? EventId { get; set; }
        public int? PackageId { get; set; }
        public int? PhotographerId { get; set; }

        [Required]
        public string Message { get; set; }

        public string? Response { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = "open"; // open, resolved

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Event? Event { get; set; }
        public virtual Package? Package { get; set; }
        public virtual Photographer? Photographer { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
} 