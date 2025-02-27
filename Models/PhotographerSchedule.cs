using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class PhotographerSchedule
    {
        [Key]
        public int Id { get; set; }

        public int PhotographerId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public string Status { get; set; } = "available"; // available, unavailable, booked

        // Navigation property
        public virtual Photographer Photographer { get; set; }
    }
} 