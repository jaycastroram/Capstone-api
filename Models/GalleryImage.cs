using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class GalleryImage
    {
        [Key]
        public int Id { get; set; }

        public int GalleryId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string? Description { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Gallery Gallery { get; set; }
        public virtual ICollection<ImageComment> Comments { get; set; } = new List<ImageComment>();
    }
} 