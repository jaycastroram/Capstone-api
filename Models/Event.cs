using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string? Description { get; set; }
        
        public int PhotographerId { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        // Navigation properties
        public virtual Photographer Photographer { get; set; }
        public virtual ICollection<EventStyle> Styles { get; set; } = new List<EventStyle>();
        public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();
    }
} 