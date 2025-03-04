using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Api.Models
{
    public class Inquiry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int PhotographerId { get; set; }

        [Required]
        public int PackageId { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string Status { get; set; } = "pending"; // pending, approved, closed, completed

        public string? Response { get; set; }

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "timestamp with time zone")]
        public DateTime? PreferredDate { get; set; }

        public string? EventType { get; set; }
        public string? Location { get; set; }
        public int? EstimatedDuration { get; set; }
        public decimal? Budget { get; set; }
        public string? SpecialRequirements { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Photographer Photographer { get; set; }
        public virtual Package Package { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
} 