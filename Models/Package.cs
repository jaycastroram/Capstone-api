using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class Package
    {
        [Key]
        public int Id { get; set; }

        public int PhotographerId { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        // Navigation properties
        public virtual Photographer Photographer { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
} 