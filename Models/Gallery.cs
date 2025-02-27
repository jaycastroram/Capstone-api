using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class Gallery
    {
        [Key]
        public int Id { get; set; }

        public int PhotographerId { get; set; }

        [Required]
        public string Title { get; set; }

        public string? AccessCode { get; set; }

        // Navigation properties
        public virtual Photographer Photographer { get; set; }
        public virtual ICollection<GalleryImage> Images { get; set; } = new List<GalleryImage>();
        public virtual ICollection<GalleryComment> Comments { get; set; } = new List<GalleryComment>();
        public virtual ICollection<GalleryStyle> Styles { get; set; } = new List<GalleryStyle>();
    }
} 