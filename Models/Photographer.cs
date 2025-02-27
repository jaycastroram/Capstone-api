using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Api.Models
{
    public class Photographer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        
        [Required]
        public string Name { get; set; }

        public string? PortfolioLink { get; set; }
        public string? ProfileImage { get; set; }
        public string? Bio { get; set; }
        public string? ContactInfo { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<Gallery> Galleries { get; set; } = new List<Gallery>();
        public virtual ICollection<PhotographerSchedule> Schedule { get; set; } = new List<PhotographerSchedule>();
        public virtual ICollection<PhotographerStyle> Styles { get; set; } = new List<PhotographerStyle>();
    }
} 