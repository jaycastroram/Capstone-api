using System.ComponentModel.DataAnnotations;

namespace Capstone.Api.Models
{
    public class ImageComment
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public int GalleryImageId { get; set; }

        [Required]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual User User { get; set; }
        public virtual GalleryImage GalleryImage { get; set; }
    }
} 