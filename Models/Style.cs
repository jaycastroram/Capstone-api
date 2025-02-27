using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class Style
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Navigation properties
        public virtual ICollection<PhotographerStyle> Photographers { get; set; } = new List<PhotographerStyle>();
        public virtual ICollection<EventStyle> Events { get; set; } = new List<EventStyle>();
        public virtual ICollection<GalleryStyle> Galleries { get; set; } = new List<GalleryStyle>();
    }
} 